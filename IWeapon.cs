
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IWeapon




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IWeapon : 
    IUpgradable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    PersonalWeaponRange Range { get; set; }

    int Damage { get; set; }

    int DamageModifier { get; set; }

    int DamageExponentiator { get; set; }

    int DamageMultiplier { get; }

    List<string> StatsModifyingDamage { get; }
  }
}
