
// Type: com.digitalarcsystems.Traveller.ASyncFileUtility`1




using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public static class ASyncFileUtility<T>
  {
    private static Queue<ASyncFileUtility<T>.Job> jobs = new Queue<ASyncFileUtility<T>.Job>();
    private static Thread thread;
    private static EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

    static ASyncFileUtility()
    {
      ASyncFileUtility<T>.thread = new Thread(new ThreadStart(ASyncFileUtility<T>.Run));
      ASyncFileUtility<T>.thread.Start();
    }

    public static void WriteAsync(string filePath, T obj, Func<object, byte[]> serializer)
    {
      lock (ASyncFileUtility<T>.jobs)
      {
        ASyncFileUtility<T>.jobs.Enqueue(new ASyncFileUtility<T>.Job()
        {
          obj = (object) obj,
          path = filePath,
          serializer = serializer
        });
        ASyncFileUtility<T>.handle.Set();
      }
    }

    private static void Run()
    {
      while (true)
      {
        ASyncFileUtility<T>.handle.WaitOne();
        try
        {
          lock (ASyncFileUtility<T>.jobs)
          {
            ASyncFileUtility<T>.Job job = ASyncFileUtility<T>.jobs.Dequeue();
            File.WriteAllBytes(job.path, job.serializer(job.obj));
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.Message);
        }
      }
    }

    public static byte[] ReadSync(string path)
    {
      lock (ASyncFileUtility<T>.jobs)
        return File.Exists(path) ? File.ReadAllBytes(path) : new byte[0];
    }

    private struct Job
    {
      public string path;
      public object obj;
      public Func<object, byte[]> serializer;
    }
  }
}
