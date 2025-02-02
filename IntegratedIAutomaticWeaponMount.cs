
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IntegratedIAutomaticWeaponMount




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class IntegratedIAutomaticWeaponMount : 
    IntegratedIMultishotRangedWeaponMount,
    IAutomaticWeapon,
    IMultishotRangedWeapon,
    IRangedWeapon,
    IRangedEquipment,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IWeapon,
    IUpgradable,
    IConsumer
  {
    [JsonIgnore]
    protected IAutomaticWeapon WrappedAutomaticWeapon
    {
      get => (IAutomaticWeapon) this.WrappedWeapon;
      set => this.WrappedWeapon = (IWeapon) value;
    }

    [JsonConstructor]
    public IntegratedIAutomaticWeaponMount()
    {
    }

    public IntegratedIAutomaticWeaponMount(IAutomaticWeapon automaticWeapon, int cost)
      : base((IMultishotRangedWeapon) automaticWeapon, cost)
    {
      this.WrappedAutomaticWeapon = automaticWeapon;
    }

    [JsonIgnore]
    public int AutoRating
    {
      get => this.WrappedAutomaticWeapon.AutoRating;
      set => this.WrappedAutomaticWeapon.AutoRating = value;
    }

    public bool FireAuto() => this.WrappedAutomaticWeapon.FireAuto();

    public bool FireBurst() => this.WrappedAutomaticWeapon.FireBurst();
  }
}
