
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IAmmunition




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IAmmunition : 
    IConsumable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IRangedEquipment,
    IModifier<IMultishotRangedWeapon>
  {
    string DamageString { get; set; }

    int CalculatePrice(IMultishotRangedWeapon equipmentToBeAddedTo);
  }
}
