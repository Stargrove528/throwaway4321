
// Type: com.digitalarcsystems.Traveller.DownloadCompleteEventArgs




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class DownloadCompleteEventArgs : EventArgs
  {
    public AssetType AssetType { get; set; }

    public DownloadCompleteEventArgs(AssetType type) => this.AssetType = type;
  }
}
