
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Shield




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Shield : 
    Armor,
    IWeapon,
    IUpgradable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    [JsonProperty]
    protected IWeapon _weapon;

    [JsonIgnore]
    public PersonalWeaponRange Range => this._weapon.Range;

    [JsonIgnore]
    private int Damage
    {
      get => this._weapon.Damage;
      set => this._weapon.Damage = value;
    }

    [JsonIgnore]
    private int DamageModifier
    {
      get => this._weapon.DamageModifier;
      set => this._weapon.DamageModifier = value;
    }

    [JsonIgnore]
    public int DamageExponentiator
    {
      get => this._weapon.DamageExponentiator;
      set => this._weapon.DamageExponentiator = value;
    }

    [JsonIgnore]
    public int DamageMultiplier => this._weapon.DamageMultiplier;

    [JsonIgnore]
    List<string> IWeapon.StatsModifyingDamage => this._weapon.StatsModifyingDamage;

    [JsonIgnore]
    int IWeapon.Damage
    {
      get => this._weapon.Damage;
      set => this._weapon.Damage = value;
    }

    [JsonIgnore]
    int IWeapon.DamageModifier
    {
      get => this._weapon.DamageModifier;
      set => this._weapon.DamageModifier = value;
    }

    [JsonIgnore]
    int IWeapon.DamageExponentiator
    {
      get => this._weapon.DamageExponentiator;
      set => this._weapon.DamageExponentiator = value;
    }

    [JsonIgnore]
    PersonalWeaponRange IWeapon.Range
    {
      get => this._weapon.Range;
      set => this._weapon.Range = value;
    }

    public Shield() => this._weapon = (IWeapon) new Weapon();

    public Shield(Armor basedOnArmor, IWeapon basedOnWeapon)
      : base(basedOnArmor)
    {
      this._weapon = basedOnWeapon;
    }

    [JsonConstructor]
    public Shield(IWeapon _basedOnMe, List<string> allowedUpgrades)
      : base(allowedUpgrades)
    {
      this._weapon = _basedOnMe;
    }
  }
}
