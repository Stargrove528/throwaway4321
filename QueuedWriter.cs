
// Type: com.digitalarcsystems.Traveller.QueuedWriter




using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class QueuedWriter
  {
    private static readonly byte[] Iv = new byte[16]
    {
      (byte) 233,
      (byte) 193,
      (byte) 128,
      (byte) 75,
      (byte) 176,
      (byte) 243,
      (byte) 223,
      (byte) 158,
      (byte) 227,
      (byte) 135,
      (byte) 95,
      (byte) 41,
      (byte) 221,
      (byte) 252,
      (byte) 160,
      (byte) 107
    };
    private static volatile object _lockObject = new object();
    private static QueuedWriter _queuedWriter;
    private static Rijndael rijndael = Rijndael.Create();
    private readonly Dictionary<string, QueuedWriter.WorkerQueue> _workQueue = new Dictionary<string, QueuedWriter.WorkerQueue>();

    public static QueuedWriter Instance
    {
      get
      {
        if (QueuedWriter._queuedWriter == null)
        {
          lock (QueuedWriter._lockObject)
            QueuedWriter._queuedWriter = new QueuedWriter();
        }
        return QueuedWriter._queuedWriter;
      }
    }

    public static int UserID { get; set; }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static string Decrypt(string filePath)
    {
      using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, QueuedWriter.rijndael.CreateDecryptor(QueuedWriter.Pw, QueuedWriter.Iv), CryptoStreamMode.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream) cryptoStream, Encoding.UTF8, false, 1024))
            return streamReader.ReadToEnd();
        }
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void Write(string filePath, string contents, bool encryptData)
    {
      using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
      {
        byte[] bytes = Encoding.UTF8.GetBytes(contents);
        if (encryptData)
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, QueuedWriter.rijndael.CreateEncryptor(QueuedWriter.Pw, QueuedWriter.Iv), CryptoStreamMode.Write))
            cryptoStream.Write(bytes, 0, bytes.Length);
        }
        else
          fileStream.Write(bytes, 0, bytes.Length);
      }
      GC.Collect();
    }

    public void Queue(string filePath, string serializedObject, bool isEncrypted)
    {
      lock (QueuedWriter._lockObject)
      {
        QueuedWriter.WorkerQueue workerQueue;
        if (!this._workQueue.TryGetValue(filePath, out workerQueue))
        {
          workerQueue = new QueuedWriter.WorkerQueue(filePath, isEncrypted);
          this._workQueue.Add(filePath, workerQueue);
        }
        workerQueue.Queue(serializedObject);
      }
    }

    private static byte[] Pw
    {
      get
      {
        return ((IEnumerable<byte>) Encoding.UTF8.GetBytes((QueuedWriter.UserID.ToString() + "COMSTOCK").SHA1())).Take<byte>(16).ToArray<byte>();
      }
    }

    private QueuedWriter()
    {
    }

    private class WorkerQueue : IDisposable
    {
      private static volatile object _queueLockObject = new object();
      private readonly bool _isEncrypted;
      private bool _disposed;
      private readonly string _filePath;
      private Thread _thread;
      private readonly System.Collections.Generic.Queue<string> _workQueue = new System.Collections.Generic.Queue<string>();

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      public void Queue(string serializedObject)
      {
        lock (QueuedWriter.WorkerQueue._queueLockObject)
        {
          this._workQueue.Enqueue(serializedObject);
          if (this._thread != null && this._thread.IsAlive)
            return;
          this._thread = new Thread(new ThreadStart(this.Worker))
          {
            IsBackground = false
          };
          this._thread.Start();
        }
      }

      public WorkerQueue(string filePath, bool isEncrypted)
      {
        this._filePath = filePath;
        this._isEncrypted = isEncrypted;
      }

      ~WorkerQueue() => this.Dispose(false);

      private void Dispose(bool disposing)
      {
        if (this._disposed)
          return;
        if (disposing && this._thread != null && this._thread.IsAlive)
          this._thread.Join(60000);
        this._disposed = true;
      }

      private void Worker()
      {
        while (true)
        {
          string contents;
          lock (QueuedWriter.WorkerQueue._queueLockObject)
          {
            if (this._workQueue.Count == 0)
              break;
            while (this._workQueue.Count > 1)
              this._workQueue.Dequeue();
            contents = this._workQueue.Dequeue();
          }
          if (!File.Exists(this._filePath))
          {
            QueuedWriter.Write(this._filePath, contents, this._isEncrypted);
          }
          else
          {
            lock (QueuedWriter.WorkerQueue._queueLockObject)
            {
              string filePath = Path.ChangeExtension(this._filePath, ".new");
              Path.ChangeExtension(this._filePath, ".bak");
              QueuedWriter.Write(filePath, contents, this._isEncrypted);
              QueuedWriter.Write(this._filePath, contents, this._isEncrypted);
            }
          }
          Thread.Sleep(1);
        }
      }
    }
  }
}
