
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Weapon




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Weapon : 
    UpgradableEquipment,
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
    private List<string> _statsModifyingDamage;

    [JsonProperty]
    public PersonalWeaponRange Range { get; set; }

    public int Damage { get; set; }

    [JsonProperty]
    public int DamageModifier { get; set; }

    [JsonProperty]
    public int DamageExponentiator { get; set; }

    [JsonIgnore]
    public int DamageMultiplier
    {
      get
      {
        int damageMultiplier = 1;
        for (int index = 0; index < this.DamageExponentiator - 1; ++index)
          damageMultiplier *= 10;
        return damageMultiplier;
      }
    }

    [JsonIgnore]
    public List<string> StatsModifyingDamage
    {
      get => this._statsModifyingDamage;
      internal set => this._statsModifyingDamage = value;
    }

    [JsonConstructor]
    protected Weapon(List<string> statsModifyingDamage)
    {
      this._statsModifyingDamage = statsModifyingDamage;
    }

    public Weapon()
    {
      this._statsModifyingDamage = new List<string>()
      {
        "STR"
      };
      this.Name = nameof (Weapon);
      this.Range = PersonalWeaponRange.Melee_Unarmed;
    }

    public Weapon(IWeapon copyMe)
      : base((IUpgradable) copyMe)
    {
      this._statsModifyingDamage = copyMe.StatsModifyingDamage;
      if (this._statsModifyingDamage == null)
        this._statsModifyingDamage = new List<string>()
        {
          "STR"
        };
      this.Damage = copyMe.Damage;
      this.DamageModifier = copyMe.DamageModifier;
      this.DamageExponentiator = copyMe.DamageExponentiator;
    }

    public Weapon(string name)
    {
      this._statsModifyingDamage = new List<string>()
      {
        "STR"
      };
      this.Name = name;
    }

    public virtual Weapon SetRange(PersonalWeaponRange range)
    {
      this.Range = range;
      return this;
    }
  }
}
