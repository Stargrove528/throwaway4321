
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.CalculatedPriceArmorOption




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class CalculatedPriceArmorOption : PriceCalculatingEquipmentOption
  {
    [JsonConstructor]
    public CalculatedPriceArmorOption()
    {
    }

    public CalculatedPriceArmorOption(string costFormula)
      : base(costFormula)
    {
    }

    public CalculatedPriceArmorOption(string costFormula, AbstractEquipmentOption copyMe)
      : base(costFormula, copyMe)
    {
    }
  }
}
