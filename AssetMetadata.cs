
// Type: com.digitalarcsystems.Traveller.DataModel.AssetMetadata




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class AssetMetadata
  {
    public string StoredHash { get; set; }

    public string Location { get; set; }

    public DateTime LastModified { get; set; }

    [JsonProperty]
    public IAssetBase TheAsset { get; set; }

    [JsonIgnore]
    public byte[] AssetBytes => this.TheAsset.AsByteArray();

    private void AssetChanged(object s, EventArgs e) => this.LastModified = DateTime.UtcNow;

    public void Initialize()
    {
      if (this.TheAsset == null)
        return;
      if (string.IsNullOrEmpty(this.StoredHash))
        this.StoredHash = this.TheAsset.AsByteArray().SHA1();
      if (this.TheAsset.SHA1<IAssetBase>() != this.StoredHash)
      {
        this.LastModified = DateTime.UtcNow;
        this.StoredHash = this.TheAsset.AsByteArray().SHA1();
      }
    }

    public void Dispose()
    {
    }
  }
}
