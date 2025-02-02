
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ExtendedAmmunition




using Newtonsoft.Json;
using System;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ExtendedAmmunition : WeaponOption
  {
    [JsonProperty]
    private int old_capacity = -1;
    [JsonProperty]
    private int old_slots = -1;

    [JsonConstructor]
    public ExtendedAmmunition()
    {
    }

    public override void ActionsOnAdd(IEquipment addedToThis)
    {
      IntegratedIMultishotRangedWeaponMount rangedWeaponMount = addedToThis as IntegratedIMultishotRangedWeaponMount;
      this.old_capacity = rangedWeaponMount.Capacity;
      this.old_slots = rangedWeaponMount.Slots;
      rangedWeaponMount.Capacity *= 11;
      rangedWeaponMount.Slots *= 2;
      base.ActionsOnAdd(addedToThis);
    }

    public override void ActionsOnRemoval(IEquipment removedFromThis)
    {
      IntegratedIMultishotRangedWeaponMount rangedWeaponMount = removedFromThis as IntegratedIMultishotRangedWeaponMount;
      if (!rangedWeaponMount.Options.Any<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => o is ExtendedAmmunition)))
      {
        rangedWeaponMount.Capacity = this.old_capacity;
        rangedWeaponMount.Slots = this.old_slots;
      }
      base.ActionsOnRemoval(removedFromThis);
    }

    public override int CalculatePrice(IEquipment weaponToBeAddedTo)
    {
      return (weaponToBeAddedTo as IntegratedIMultishotRangedWeaponMount).StandardConsumableCost * 9 * 2;
    }
  }
}
