
// Type: com.digitalarcsystems.Traveller.DataModel.IGenericAsset`1




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface IGenericAsset<out T> : IAsset, IDescribable, IAssetBase
  {
    T Contents { get; }
  }
}
