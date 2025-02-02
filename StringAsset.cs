
// Type: com.digitalarcsystems.Traveller.DataModel.StringAsset




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class StringAsset : IGenericAsset<string>, IAsset, IDescribable, IAssetBase
  {
    public string Description { get; set; }

    public string Name { get; set; }

    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    public string Contents { get; set; }

    [JsonConstructor]
    public StringAsset()
    {
      this.Tags = new List<AssetTag>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
    }
  }
}
