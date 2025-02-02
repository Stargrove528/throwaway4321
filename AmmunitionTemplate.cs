
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.AmmunitionTemplate




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class AmmunitionTemplate : 
    Consumable,
    IAmmunition,
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
    [JsonProperty]
    private List<IAmmoAction> actions = new List<IAmmoAction>();

    [JsonProperty]
    public string Cost_Formula { get; set; }

    public int RangeInMeters { get; set; }

    public string DamageString { get; set; }

    public void addIAmmoAction(IAmmoAction addMe) => this.actions.Add(addMe);

    [JsonConstructor]
    public AmmunitionTemplate()
    {
    }

    public AmmunitionTemplate(params IAmmoAction[] ammo_actions)
    {
      this.actions.AddRange((IEnumerable<IAmmoAction>) ammo_actions);
    }

    public AmmunitionTemplate(IEquipment copyMe)
      : base(copyMe)
    {
    }

    public void ActionsOnAdd(IMultishotRangedWeapon addedToThis)
    {
      foreach (IAmmoAction action in this.actions)
        action.ActionsOnAdd(addedToThis);
    }

    public void ActionsOnRemoval(IMultishotRangedWeapon removedFromThis)
    {
      foreach (IAmmoAction action in this.actions)
        action.ActionsOnRemoval(removedFromThis);
    }

    public int CalculatePrice(IMultishotRangedWeapon equipmentToBeAddedTo)
    {
      return Utility.calculateValue(this.Cost_Formula, (object) equipmentToBeAddedTo);
    }
  }
}
