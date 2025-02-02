
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IConsumable




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IConsumable : 
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    bool Rechargable { get; set; }

    string ConsumableType { get; set; }

    string AmountName { get; set; }

    int MaxAmount { get; set; }

    int CurrentAmount { get; set; }
  }
}
