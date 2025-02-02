
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.WeaponAugment




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class WeaponAugment : 
    Augmentation,
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
    private List<string> _statsModifyingDamage = new List<string>();

    [JsonConstructor]
    public WeaponAugment(
      List<string> statsModifyingWeaponDamage,
      List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
      this.StatsModifyingDamage = statsModifyingWeaponDamage;
    }

    public WeaponAugment() => this.StatsModifyingDamage.Add("STR");

    public WeaponAugment(IAugmentation copyMe)
      : base(copyMe)
    {
    }

    public int Damage { get; set; }

    public int DamageExponentiator { get; set; }

    public int DamageModifier { get; set; }

    public int DamageMultiplier { get; set; }

    public PersonalWeaponRange Range { get; set; }

    [JsonIgnore]
    public List<string> StatsModifyingDamage
    {
      get => this._statsModifyingDamage;
      internal set => this._statsModifyingDamage = value;
    }
  }
}
