
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.RangedWeapon




using Newtonsoft.Json;
using System.Globalization;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class RangedWeapon : 
    Weapon,
    IRangedWeapon,
    IRangedEquipment,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IWeapon,
    IUpgradable
  {
    [JsonConstructor]
    public RangedWeapon() => this.StatsModifyingDamage.Clear();

    public RangedWeapon(IWeapon copyMe)
      : base(copyMe)
    {
      this.StatsModifyingDamage.Clear();
    }

    public RangedWeapon(IRangedWeapon copyMe)
      : base((IWeapon) copyMe)
    {
      this.StatsModifyingDamage.Clear();
      this.RangeInMeters = copyMe.RangeInMeters;
    }

    public int RangeInMeters { get; set; }

    public string GetEffectiveRange(int distanceToTarget, bool isInCombat = false)
    {
      if (this.RangeInMeters == 0)
        return "";
      if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(this.Traits, "Scope", CompareOptions.IgnoreCase) < 0 && (isInCombat && distanceToTarget > 100 || !isInCombat && distanceToTarget > 300))
        return "Extreme Range";
      if ((double) distanceToTarget <= (double) this.RangeInMeters / 4.0)
        return "Short Range";
      if (distanceToTarget > this.RangeInMeters && distanceToTarget <= this.RangeInMeters * 2)
        return "Long Range";
      return distanceToTarget > this.RangeInMeters * 2 && distanceToTarget <= this.RangeInMeters * 4 ? "Extreme Range" : "";
    }
  }
}
