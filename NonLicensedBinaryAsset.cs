
// Type: com.digitalarcsystems.Traveller.DataModel.NonLicensedBinaryAsset




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class NonLicensedBinaryAsset : BinaryAssetBase, INonLicensedAsset, IAssetBase
  {
    public string DefaultFileName => this.Id.ToString("D") + ".trnlba";

    public int CreatingUser { get; set; }

    public static NonLicensedBinaryAsset CreateFrom(
      byte[] bytes,
      int creatingUser,
      params AssetTag[] tags)
    {
      NonLicensedBinaryAsset from = new NonLicensedBinaryAsset();
      from.Id = Guid.NewGuid();
      from.Contents = bytes;
      from.Tags = ((IEnumerable<AssetTag>) tags).ToList<AssetTag>();
      from.CreatingUser = creatingUser;
      return from;
    }

    [JsonConstructor]
    public NonLicensedBinaryAsset()
    {
    }
  }
}
