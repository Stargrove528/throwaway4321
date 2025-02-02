
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IMultishotRangedWeapon




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IMultishotRangedWeapon : 
    IRangedWeapon,
    IRangedEquipment,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IWeapon,
    IUpgradable,
    IConsumer
  {
    bool Fire();
  }
}
