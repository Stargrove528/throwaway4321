
// Type: com.digitalarcsystems.Traveller.DownloadProgressEventArgs




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class DownloadProgressEventArgs : EventArgs
  {
    public long BytesDownloaded { get; private set; }

    public long TotalBytes { get; private set; }

    public string LastDownloadName { get; private set; }

    public string Error { get; private set; }

    public AssetType AssetType { get; set; }

    public DownloadProgressEventArgs(
      long downloaded,
      long total,
      string lastName,
      string error,
      AssetType type)
    {
      this.BytesDownloaded = downloaded;
      this.TotalBytes = total;
      this.LastDownloadName = lastName;
      this.Error = error;
      this.AssetType = type;
    }
  }
}
