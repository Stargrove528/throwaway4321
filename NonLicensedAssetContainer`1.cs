
// Type: com.digitalarcsystems.Traveller.DataModel.NonLicensedAssetContainer`1




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class NonLicensedAssetContainer<T> : 
    INonLicensedAsset,
    IAssetBase,
    IGenericAsset<IEnumerable<T>>,
    IAsset,
    IDescribable
  {
    public Guid Id { get; set; }

    public string DefaultFileName => this.Id.ToString("D") + ".trnlba";

    public int CreatingUser { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    public IEnumerable<T> Contents { get; set; }

    [JsonConstructor]
    public NonLicensedAssetContainer(int creatingUser)
    {
      this.Id = Guid.NewGuid();
      this.CreatingUser = creatingUser;
      this.Tags = new List<AssetTag>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.Contents = (IEnumerable<T>) new List<T>();
    }
  }
}
