
// Type: com.digitalarcsystems.Traveller.DataModel.BinaryAssetBase




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class BinaryAssetBase : IGenericAsset<byte[]>, IAsset, IDescribable, IAssetBase
  {
    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public virtual byte[] Contents { get; set; }

    [JsonProperty]
    public string Description { get; set; }

    public string ContentHash { get; set; }

    public Guid Id { get; set; }

    [JsonProperty]
    public string Name { get; set; }

    [JsonProperty]
    public List<AssetTag> Tags { get; set; }

    public string ClientFilePath { get; set; }

    [JsonConstructor]
    public BinaryAssetBase()
    {
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Tags = new List<AssetTag>();
    }
  }
}
