
// Type: com.digitalarcsystems.Traveller.DataModel.IAsset




using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface IAsset : IDescribable, IAssetBase
  {
    Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    List<AssetTag> Tags { get; set; }
  }
}
