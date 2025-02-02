
// Type: com.digitalarcsystems.Traveller.SyncEventArgs




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class SyncEventArgs : EventArgs
  {
    public AssetType Type { get; set; }

    public SyncEventArgs(AssetType type) => this.Type = type;
  }
}
