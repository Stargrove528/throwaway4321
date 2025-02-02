
// Type: com.digitalarcsystems.Traveller.CacheLocationManager




using System;
using System.IO;
using System.Threading;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class CacheLocationManager
  {
    private const string LOCKFILENAME = "/lockFile";
    private bool heartIsBeating = false;
    private static string lockFilePath;
    private string cachePath;
    private static CacheLocationManager clm;
    private Thread heartBeatThread;
    private static DateTime epoch = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private CacheLocationManager(string path)
    {
      this.cachePath = path;
      CacheLocationManager.lockFilePath = this.cachePath + "/lockFile";
      CacheLocationManager.clm = this;
    }

    private void Update()
    {
      while (this.heartIsBeating)
      {
        long totalMilliseconds = (long) (DateTime.UtcNow - CacheLocationManager.epoch).TotalMilliseconds;
        string path = "";
        string lockFilePath = CacheLocationManager.lockFilePath;
        char[] chArray = new char[2]{ '\\', '/' };
        foreach (string str in lockFilePath.Split(chArray))
        {
          path = !(path != "") ? str : CacheLocationManager.lockFilePath.Substring(0, CacheLocationManager.lockFilePath.IndexOf(str, path.Length) + str.Length);
          if (str.Length > 0 && !str.Contains("/lockFile".Substring(1)) && !Directory.Exists(path))
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(CacheLocationManager.lockFilePath, "TCG " + totalMilliseconds.ToString());
        Thread.Sleep(2000);
      }
    }

    public static string whoIsRunning()
    {
      if (!File.Exists(CacheLocationManager.lockFilePath))
        return (string) null;
      string str = File.ReadAllText(CacheLocationManager.lockFilePath);
      try
      {
        long num = long.Parse(str.Substring(3));
        if ((long) (DateTime.UtcNow - CacheLocationManager.epoch).TotalMilliseconds - num > 5000L)
          return (string) null;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
      return str.Substring(0, 3);
    }

    public void startHeartBeat()
    {
      this.heartIsBeating = true;
      this.heartBeatThread = new Thread((ThreadStart) (() => this.Update()));
      this.heartBeatThread.Start();
    }

    public void endHeartBeat()
    {
      this.heartIsBeating = false;
      this.heartBeatThread.Join(2000);
    }

    public static CacheLocationManager getCacheLocationManager(string path)
    {
      return CacheLocationManager.clm != null ? CacheLocationManager.clm : new CacheLocationManager(path);
    }
  }
}
