
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IRangedWeapon




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IRangedWeapon : 
    IRangedEquipment,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IWeapon,
    IUpgradable
  {
    string GetEffectiveRange(int distanceToTarget, bool isInCombat = false);
  }
}
