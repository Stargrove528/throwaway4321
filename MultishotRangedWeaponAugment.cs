
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.MultishotRangedWeaponAugment




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class MultishotRangedWeaponAugment : 
    WeaponAugment,
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
    private readonly IMultishotRangedWeapon _rangedWeapon = (IMultishotRangedWeapon) new MultishotRangedWeapon();

    [JsonConstructor]
    public MultishotRangedWeaponAugment(List<string> allowedUpgradeCategories)
    {
      this.AllowedUpgradeCategories = allowedUpgradeCategories;
    }

    public MultishotRangedWeaponAugment() => this.StatsModifyingDamage.Clear();

    public MultishotRangedWeaponAugment(WeaponAugment copyMe)
      : base((IAugmentation) copyMe)
    {
      this.StatsModifyingDamage.Clear();
      this.Damage = copyMe.Damage;
      this.DamageExponentiator = copyMe.DamageExponentiator;
      this.DamageModifier = copyMe.DamageModifier;
      this.DamageMultiplier = copyMe.DamageMultiplier;
      this.Range = copyMe.Range;
    }

    public MultishotRangedWeaponAugment(WeaponAugment copyMe, IMultishotRangedWeapon rangedWeapon)
      : this(copyMe)
    {
      this.StatsModifyingDamage.Clear();
      this._rangedWeapon = rangedWeapon;
    }

    public MultishotRangedWeaponAugment(MultishotRangedWeaponAugment copyMe)
      : this((WeaponAugment) copyMe)
    {
      this.StatsModifyingDamage.Clear();
      this.RangeInMeters = copyMe.RangeInMeters;
      this.Capacity = copyMe.Capacity;
      this.StandardConsumableCost = copyMe.StandardConsumableCost;
      this.ConsumableTypeAllowed = copyMe.ConsumableTypeAllowed;
      this.ConsumableAmountName = copyMe.ConsumableAmountName;
    }

    public int Capacity
    {
      get => this._rangedWeapon.Capacity;
      set => this._rangedWeapon.Capacity = value;
    }

    public string ConsumableAmountName
    {
      get => this._rangedWeapon.ConsumableAmountName;
      set => this._rangedWeapon.ConsumableAmountName = value;
    }

    public IConsumable ConsumableLoaded => this._rangedWeapon.ConsumableLoaded;

    public string ConsumableTypeAllowed
    {
      get => this._rangedWeapon.ConsumableTypeAllowed;
      set => this._rangedWeapon.ConsumableTypeAllowed = value;
    }

    public int RangeInMeters
    {
      get => this._rangedWeapon.RangeInMeters;
      set => this._rangedWeapon.RangeInMeters = value;
    }

    public int StandardConsumableCost
    {
      get => this._rangedWeapon.StandardConsumableCost;
      set => this._rangedWeapon.StandardConsumableCost = value;
    }

    public bool UnlimitedAmmunition
    {
      get => this._rangedWeapon.UnlimitedAmmunition;
      set => this._rangedWeapon.UnlimitedAmmunition = value;
    }

    public bool ConsumableIsRechargable
    {
      get => this._rangedWeapon.ConsumableIsRechargable;
      set => this._rangedWeapon.ConsumableIsRechargable = value;
    }

    public bool Fire() => this._rangedWeapon.Fire();

    public string GetEffectiveRange(int distanceToTarget, bool isInCombat = false)
    {
      return this._rangedWeapon.GetEffectiveRange(distanceToTarget, isInCombat);
    }

    public bool Load(IConsumable loadMe) => this._rangedWeapon.Load(loadMe);

    public bool Unload() => this._rangedWeapon.Unload();

    public bool Use() => this._rangedWeapon.Use();

    public bool Use(int amount) => this._rangedWeapon.Use(amount);
  }
}
