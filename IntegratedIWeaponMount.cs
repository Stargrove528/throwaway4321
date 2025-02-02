
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IntegratedIWeaponMount




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class IntegratedIWeaponMount : 
    BattleDressMod,
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
    protected List<IEquipmentOption> _options = new List<IEquipmentOption>();

    [JsonProperty]
    protected IWeapon WrappedWeapon { get; set; }

    [JsonConstructor]
    public IntegratedIWeaponMount()
    {
    }

    public IntegratedIWeaponMount(IWeapon meleeWeapon, int cost)
    {
      FixedPriceEquipmentOption basedOnMe = new FixedPriceEquipmentOption((IEquipment) meleeWeapon);
      basedOnMe.Cost = cost;
      // ISSUE: explicit constructor call
      base.\u002Ector((IEquipmentOption) basedOnMe);
      this.WrappedWeapon = meleeWeapon;
      this.Description = meleeWeapon.Description;
      this.Name = meleeWeapon.Name;
      this.UpgradeCategories = new List<string>()
      {
        "Battle Dress Mod"
      };
    }

    [JsonIgnore]
    public new string Traits
    {
      get => this.WrappedWeapon.Traits;
      set => this.WrappedWeapon.Traits = value;
    }

    [JsonIgnore]
    public Guid WeaponId => this.WrappedWeapon.Id;

    [JsonIgnore]
    public PersonalWeaponRange Range
    {
      get => this.WrappedWeapon.Range;
      set => this.WrappedWeapon.Range = value;
    }

    [JsonIgnore]
    public int Damage
    {
      get => this.WrappedWeapon.Damage;
      set => this.WrappedWeapon.Damage = value;
    }

    [JsonIgnore]
    public int DamageModifier
    {
      get => this.WrappedWeapon.DamageModifier;
      set => this.WrappedWeapon.DamageModifier = value;
    }

    [JsonIgnore]
    public int DamageExponentiator
    {
      get => this.WrappedWeapon.DamageExponentiator;
      set => this.WrappedWeapon.DamageExponentiator = value;
    }

    [JsonIgnore]
    public int DamageMultiplier => this.WrappedWeapon.DamageMultiplier;

    [JsonIgnore]
    public List<string> StatsModifyingDamage => this.WrappedWeapon.StatsModifyingDamage;

    [JsonIgnore]
    public List<string> AllowedUpgradeCategories => this.WrappedWeapon.AllowedUpgradeCategories;

    [JsonIgnore]
    public List<IEquipmentOption> Options
    {
      get
      {
        List<IEquipmentOption> source = new List<IEquipmentOption>((IEnumerable<IEquipmentOption>) this.WrappedWeapon.Options);
        if (this._options.Any<IEquipmentOption>())
          source.AddRange((IEnumerable<IEquipmentOption>) this._options);
        source.OrderBy<IEquipmentOption, string>((Func<IEquipmentOption, string>) (o => o.Name));
        return source;
      }
    }

    [JsonIgnore]
    public List<IWeapon> WeaponSubcomponents => this.WrappedWeapon.WeaponSubcomponents;

    [JsonIgnore]
    public List<IComputer> ComputerSubcomponents => this.WrappedWeapon.ComputerSubcomponents;

    public bool AddOption(IEquipmentOption addMe)
    {
      bool flag = false;
      if (Utility.anyCommaSeperatedMatches(this.AllowedUpgradeCategories, addMe.UpgradeCategories))
      {
        addMe.ActionsOnAdd((IEquipment) this);
        this._options.Add(addMe);
        flag = true;
      }
      return flag;
    }

    public void RemoveOption(IEquipmentOption removeMe)
    {
      this.WrappedWeapon.RemoveOption(removeMe);
      if (!this._options.Contains(removeMe))
        return;
      this._options.Remove(removeMe);
      removeMe.ActionsOnRemoval((IEquipment) this);
    }
  }
}
