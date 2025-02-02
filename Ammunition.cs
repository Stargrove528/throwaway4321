
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Ammunition




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Ammunition : 
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
    protected AmmunitionTemplate ammunitionTemplate;

    [JsonConstructor]
    public Ammunition()
    {
    }

    public Ammunition(AmmunitionTemplate ammoTemplate) => this.ammunitionTemplate = ammoTemplate;

    public int RangeInMeters { get; set; }

    public string DamageString { get; set; }

    public void ActionsOnAdd(IMultishotRangedWeapon addedToThis)
    {
      if (this.ammunitionTemplate == null)
        return;
      this.ammunitionTemplate.ActionsOnAdd(addedToThis);
    }

    public void ActionsOnRemoval(IMultishotRangedWeapon removedFromThis)
    {
      if (this.ammunitionTemplate == null)
        return;
      this.ammunitionTemplate.ActionsOnRemoval(removedFromThis);
    }

    public int CalculatePrice(IMultishotRangedWeapon equipmentToBeAddedTo)
    {
      return this.ammunitionTemplate == null ? this.Cost : this.ammunitionTemplate.CalculatePrice(equipmentToBeAddedTo);
    }
  }
}
