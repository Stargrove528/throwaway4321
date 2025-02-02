
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.UpgradeMultishotRangedWeapon




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class UpgradeMultishotRangedWeapon : 
    UpgradeWeapon,
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
    IConsumer,
    IEquipmentOption,
    IEquipmentModifier,
    IModifier<IEquipment>
  {
    [JsonProperty]
    protected IMultishotRangedWeapon _msrw;

    [JsonConstructor]
    protected UpgradeMultishotRangedWeapon()
    {
    }

    public UpgradeMultishotRangedWeapon(IMultishotRangedWeapon msrw, IEquipmentOption option)
      : base((IWeapon) msrw, option)
    {
      this._msrw = msrw;
    }

    public int Capacity
    {
      get => this._msrw.Capacity;
      set => this._msrw.Capacity = value;
    }

    public string ConsumableAmountName
    {
      get => this._msrw.ConsumableAmountName;
      set => this._msrw.ConsumableAmountName = value;
    }

    public IConsumable ConsumableLoaded => this._msrw.ConsumableLoaded;

    public string ConsumableTypeAllowed
    {
      get => this._msrw.ConsumableTypeAllowed;
      set => this._msrw.ConsumableTypeAllowed = value;
    }

    public int RangeInMeters
    {
      get => this._msrw.RangeInMeters;
      set => this._msrw.RangeInMeters = value;
    }

    public int StandardConsumableCost
    {
      get => this._msrw.StandardConsumableCost;
      set => this._msrw.StandardConsumableCost = value;
    }

    public bool UnlimitedAmmunition
    {
      get => this._msrw.UnlimitedAmmunition;
      set => this._msrw.UnlimitedAmmunition = value;
    }

    public bool ConsumableIsRechargable
    {
      get => this._msrw.ConsumableIsRechargable;
      set => this._msrw.ConsumableIsRechargable = value;
    }

    public bool Fire() => this._msrw.Fire();

    public string GetEffectiveRange(int distanceToTarget, bool isInCombat = false)
    {
      return this._msrw.GetEffectiveRange(distanceToTarget, isInCombat);
    }

    public bool Load(IConsumable loadMe) => this._msrw.Load(loadMe);

    public bool Unload() => this._msrw.Unload();

    public bool Use() => this._msrw.Use();

    public bool Use(int amount) => this._msrw.Use(amount);
  }
}
