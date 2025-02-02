
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IAutomaticWeapon




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IAutomaticWeapon : 
    IMultishotRangedWeapon,
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
    int AutoRating { get; set; }

    bool FireBurst();

    bool FireAuto();
  }
}
