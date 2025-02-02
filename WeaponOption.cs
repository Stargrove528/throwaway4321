
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.WeaponOption




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class WeaponOption : AbstractEquipmentOption
  {
    [JsonConstructor]
    public WeaponOption()
    {
    }

    public override int CalculatePrice(IEquipment weaponToBeAddedTo) => this.Cost;
  }
}
