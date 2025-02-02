
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.MultishotRangedWeapon




using Newtonsoft.Json;
using System.Globalization;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class MultishotRangedWeapon : 
    Weapon,
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
    [JsonProperty]
    private readonly Consumer _consumer = new Consumer();

    [JsonConstructor]
    public MultishotRangedWeapon() => this.StatsModifyingDamage.Clear();

    public MultishotRangedWeapon(IWeapon copyMe)
      : base(copyMe)
    {
      this.StatsModifyingDamage.Clear();
    }

    public MultishotRangedWeapon(IMultishotRangedWeapon copyMe)
      : base((IWeapon) copyMe)
    {
      this.StatsModifyingDamage.Clear();
      this.Capacity = copyMe.Capacity;
      this.ConsumableAmountName = copyMe.ConsumableAmountName;
      this.ConsumableTypeAllowed = copyMe.ConsumableTypeAllowed;
      this.StandardConsumableCost = copyMe.StandardConsumableCost;
      this.UnlimitedAmmunition = copyMe.UnlimitedAmmunition;
      this.ConsumableIsRechargable = copyMe.ConsumableIsRechargable;
    }

    [JsonIgnore]
    public IConsumable ConsumableLoaded => this._consumer.ConsumableLoaded;

    [JsonIgnore]
    public int Capacity
    {
      get => this._consumer.Capacity;
      set => this._consumer.Capacity = value;
    }

    [JsonIgnore]
    public int StandardConsumableCost
    {
      get => this._consumer.StandardConsumableCost;
      set => this._consumer.StandardConsumableCost = value;
    }

    [JsonIgnore]
    public string ConsumableAmountName
    {
      get => this._consumer.ConsumableAmountName;
      set => this._consumer.ConsumableAmountName = value;
    }

    [JsonIgnore]
    public string ConsumableTypeAllowed
    {
      get => this._consumer.ConsumableTypeAllowed;
      set => this._consumer.ConsumableTypeAllowed = value;
    }

    public bool Load(IConsumable loadMe)
    {
      bool flag = this._consumer.Load(loadMe);
      if (flag)
      {
        if (loadMe is IEquipmentModifier)
          ((IModifier<IEquipment>) loadMe).ActionsOnAdd((IEquipment) this);
        else if (loadMe is IModifier<IMultishotRangedWeapon>)
          ((IModifier<IMultishotRangedWeapon>) loadMe).ActionsOnAdd((IMultishotRangedWeapon) this);
      }
      return flag;
    }

    public bool Unload()
    {
      if (this._consumer.ConsumableLoaded is IEquipmentModifier)
        ((IModifier<IEquipment>) this._consumer.ConsumableLoaded).ActionsOnRemoval((IEquipment) this);
      else if (this._consumer.ConsumableLoaded is IAmmunition)
        ((IModifier<IMultishotRangedWeapon>) this._consumer.ConsumableLoaded).ActionsOnRemoval((IMultishotRangedWeapon) this);
      return this._consumer.Unload();
    }

    public bool Use() => this._consumer.Use();

    public bool Use(int amount) => this._consumer.Use(amount);

    public bool Fire() => this.Use();

    public int RangeInMeters { get; set; }

    public bool UnlimitedAmmunition
    {
      get => this._consumer.UnlimitedAmmunition;
      set => this._consumer.UnlimitedAmmunition = value;
    }

    public bool ConsumableIsRechargable
    {
      get => this._consumer.ConsumableIsRechargable;
      set => this._consumer.ConsumableIsRechargable = value;
    }

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
