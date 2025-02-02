
// Type: com.digitalarcsystems.Traveller.DataModel.Presentation




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Presentation : Describable, IAsset, IDescribable, IAssetBase
  {
    [JsonProperty]
    public int value;
    [JsonProperty]
    public bool isResult;

    [JsonProperty]
    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    [JsonProperty]
    public List<AssetTag> Tags { get; set; }

    [JsonProperty]
    public Guid Id { get; set; }

    [JsonConstructor]
    public Presentation()
    {
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Tags = new List<AssetTag>();
      this.Id = Guid.NewGuid();
    }

    public Presentation(IDescribable useMyNameAndDescription)
      : this(useMyNameAndDescription.Name, useMyNameAndDescription.Description)
    {
    }

    public Presentation(string name, string description)
      : base(name, description)
    {
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Tags = new List<AssetTag>();
      this.Id = Guid.NewGuid();
    }
  }
}
