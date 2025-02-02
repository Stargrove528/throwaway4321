
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Grenade




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Grenade : 
    RangedWeapon,
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
    private IAmmunition ammunition;
    private string originalTraits;

    public int originalDamageExponetiator { get; private set; }

    public int originalDamage { get; private set; }

    public int originalDamageModifier { get; private set; }

    [JsonConstructor]
    public Grenade(IAmmunition _ammunition) => this.ammunition = _ammunition;

    public Grenade()
    {
    }

    public Grenade(IRangedWeapon weaponCopy, IAmmunition ammoCopy)
      : base(weaponCopy)
    {
      this.ammunition = ammoCopy;
    }

    [JsonIgnore]
    public string AmountName
    {
      get => this.ammunition.AmountName;
      set => this.ammunition.AmountName = value;
    }

    [JsonIgnore]
    public string ConsumableType
    {
      get => this.ammunition.ConsumableType;
      set => this.ammunition.ConsumableType = value;
    }

    [JsonIgnore]
    public int CurrentAmount
    {
      get => this.ammunition.CurrentAmount;
      set => this.ammunition.CurrentAmount = value;
    }

    [JsonIgnore]
    public int MaxAmount
    {
      get => this.ammunition.MaxAmount;
      set => this.ammunition.MaxAmount = value;
    }

    [JsonIgnore]
    public bool Rechargable
    {
      get => false;
      set
      {
      }
    }

    [JsonIgnore]
    public string DamageString
    {
      get => this.ammunition.DamageString;
      set => this.ammunition.DamageString = value;
    }

    public void ActionsOnAdd(IMultishotRangedWeapon addedToThis)
    {
      this.originalDamage = addedToThis.Damage;
      this.originalDamageModifier = addedToThis.DamageModifier;
      this.originalDamageExponetiator = addedToThis.DamageExponentiator;
      this.originalTraits = addedToThis.Traits;
      addedToThis.Damage = this.Damage;
      addedToThis.DamageModifier = this.DamageModifier;
      addedToThis.DamageExponentiator = this.DamageExponentiator;
      foreach (EquipmentExtensions.Trait trait in this.GetTraits())
        addedToThis.addTrait(trait);
      this.ammunition.ActionsOnAdd(addedToThis);
    }

    public void ActionsOnRemoval(IMultishotRangedWeapon removedFromThis)
    {
      this.ammunition.ActionsOnRemoval(removedFromThis);
      removedFromThis.Damage = this.originalDamage;
      removedFromThis.DamageModifier = this.originalDamageModifier;
      removedFromThis.DamageExponentiator = this.originalDamageExponetiator;
      removedFromThis.Traits = this.originalTraits;
    }

    public int CalculatePrice(IMultishotRangedWeapon equipmentToBeAddedTo)
    {
      return this.ammunition.CalculatePrice(equipmentToBeAddedTo);
    }
  }
}
