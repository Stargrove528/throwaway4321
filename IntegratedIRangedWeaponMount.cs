
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IntegratedIRangedWeaponMount




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class IntegratedIRangedWeaponMount : 
    IntegratedIWeaponMount,
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
    [JsonIgnore]
    protected IRangedWeapon WrappedRangedWeapon
    {
      get => (IRangedWeapon) this.WrappedWeapon;
      set => this.WrappedWeapon = (IWeapon) value;
    }

    [JsonConstructor]
    public IntegratedIRangedWeaponMount()
    {
    }

    public IntegratedIRangedWeaponMount(IRangedWeapon rangedWeapon, int cost)
      : base((IWeapon) rangedWeapon, cost)
    {
      this.WrappedRangedWeapon = rangedWeapon;
    }

    [JsonIgnore]
    public int RangeInMeters
    {
      get => this.WrappedRangedWeapon.RangeInMeters;
      set => this.WrappedRangedWeapon.RangeInMeters = value;
    }

    public string GetEffectiveRange(int distanceToTarget, bool isInCombat = false)
    {
      return this.WrappedRangedWeapon.GetEffectiveRange(distanceToTarget, isInCombat);
    }
  }
}
