
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IUpgradable




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IUpgradable : 
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    List<string> AllowedUpgradeCategories { get; }

    List<IEquipmentOption> Options { get; }

    bool AddOption(IEquipmentOption addMe);

    void RemoveOption(IEquipmentOption removeMe);

    List<IWeapon> WeaponSubcomponents { get; }

    List<IComputer> ComputerSubcomponents { get; }
  }
}
