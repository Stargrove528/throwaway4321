
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IntegratedIMultishotRangedWeaponMount




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class IntegratedIMultishotRangedWeaponMount : 
    IntegratedIRangedWeaponMount,
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
    protected IMultishotRangedWeapon WrappedMultishotRangedWeapon
    {
      get => (IMultishotRangedWeapon) this.WrappedWeapon;
      set => this.WrappedWeapon = (IWeapon) value;
    }

    [JsonConstructor]
    public IntegratedIMultishotRangedWeaponMount()
    {
    }

    public IntegratedIMultishotRangedWeaponMount(IMultishotRangedWeapon multishotWeapon, int cost)
      : base((IRangedWeapon) multishotWeapon, cost)
    {
      this.WrappedMultishotRangedWeapon = multishotWeapon;
    }

    [JsonIgnore]
    public bool UnlimitedAmmunition
    {
      get => this.WrappedMultishotRangedWeapon.UnlimitedAmmunition;
      set => this.WrappedMultishotRangedWeapon.UnlimitedAmmunition = value;
    }

    [JsonIgnore]
    public int Capacity
    {
      get => this.WrappedMultishotRangedWeapon.Capacity;
      set => this.WrappedMultishotRangedWeapon.Capacity = value;
    }

    [JsonIgnore]
    public int StandardConsumableCost
    {
      get => this.WrappedMultishotRangedWeapon.StandardConsumableCost;
      set => this.WrappedMultishotRangedWeapon.StandardConsumableCost = value;
    }

    [JsonIgnore]
    public string ConsumableAmountName
    {
      get => this.WrappedMultishotRangedWeapon.ConsumableAmountName;
      set => this.WrappedMultishotRangedWeapon.ConsumableAmountName = value;
    }

    [JsonIgnore]
    public bool ConsumableIsRechargable
    {
      get => this.WrappedMultishotRangedWeapon.ConsumableIsRechargable;
      set => this.WrappedMultishotRangedWeapon.ConsumableIsRechargable = value;
    }

    [JsonIgnore]
    public IConsumable ConsumableLoaded => this.WrappedMultishotRangedWeapon.ConsumableLoaded;

    [JsonIgnore]
    public string ConsumableTypeAllowed
    {
      get => this.WrappedMultishotRangedWeapon.ConsumableTypeAllowed;
      set => this.WrappedMultishotRangedWeapon.ConsumableTypeAllowed = value;
    }

    public bool Fire() => this.WrappedMultishotRangedWeapon.Fire();

    public bool Load(IConsumable loadMe) => this.WrappedMultishotRangedWeapon.Load(loadMe);

    public bool Unload() => this.WrappedMultishotRangedWeapon.Unload();

    public bool Use() => this.WrappedMultishotRangedWeapon.Use();

    public bool Use(int amount) => this.WrappedMultishotRangedWeapon.Use(amount);
  }
}
