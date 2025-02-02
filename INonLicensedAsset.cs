
// Type: com.digitalarcsystems.Traveller.DataModel.INonLicensedAsset




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface INonLicensedAsset : IAssetBase
  {
    string DefaultFileName { get; }

    int CreatingUser { get; set; }
  }
}
