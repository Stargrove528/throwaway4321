
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.FixedPriceEquipmentOption




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class FixedPriceEquipmentOption : AbstractEquipmentOption
  {
    public FixedPriceEquipmentOption()
    {
    }

    public FixedPriceEquipmentOption(IEquipment copyMe)
      : base(copyMe)
    {
    }

    public override int CalculatePrice(IEquipment equipmentToBeAddedTo) => this.Cost;
  }
}
