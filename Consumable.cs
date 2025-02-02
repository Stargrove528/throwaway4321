
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Consumable




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Consumable : 
    com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment,
    IConsumable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    public Consumable(IEquipment copyMe)
      : base(copyMe)
    {
    }

    public Consumable()
    {
    }

    public string AmountName { get; set; }

    public string ConsumableType { get; set; }

    public int CurrentAmount { get; set; }

    public int MaxAmount { get; set; }

    public bool Rechargable { get; set; }
  }
}
