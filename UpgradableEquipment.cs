
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.UpgradableEquipment




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class UpgradableEquipment : 
    com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment,
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

    [JsonConstructor]
    public UpgradableEquipment()
    {
    }

    public UpgradableEquipment(IUpgradable copyMe)
      : base((IEquipment) copyMe)
    {
      if (copyMe.Options != null)
        copyMe.Options.ForEach((Action<IEquipmentOption>) (o => this.AddOption(o.Clone<IEquipmentOption>())));
      this.AllowedUpgradeCategories = copyMe.AllowedUpgradeCategories;
    }

    public UpgradableEquipment(IEquipment copyMe)
      : base(copyMe)
    {
    }

    [JsonProperty]
    public List<string> AllowedUpgradeCategories { get; set; }

    protected void setDefaultUpgradeCategories()
    {
      this.AllowedUpgradeCategories = new List<string>((IEnumerable<string>) this.Categories);
      this.AllowedUpgradeCategories.Add(this.Id.ToString());
    }

    [JsonIgnore]
    public List<IEquipmentOption> Options
    {
      get => new List<IEquipmentOption>((IEnumerable<IEquipmentOption>) this._options);
    }

    public List<IWeapon> WeaponSubcomponents
    {
      get
      {
        return this._options.Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (x => x is IWeapon)).Select<IEquipmentOption, IWeapon>((Func<IEquipmentOption, IWeapon>) (x => (IWeapon) x)).ToList<IWeapon>();
      }
    }

    [JsonIgnore]
    public List<IComputer> ComputerSubcomponents
    {
      get
      {
        return this._options.Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (x => x is IComputer)).Select<IEquipmentOption, IComputer>((Func<IEquipmentOption, IComputer>) (x => (IComputer) x)).ToList<IComputer>();
      }
    }

    public virtual bool AddOption(IEquipmentOption addMe)
    {
      bool flag = false;
      if (Utility.anyCommaSeperatedMatches(this.AllowedUpgradeCategories, addMe.UpgradeCategories))
      {
        this._options.Add(addMe);
        this.Weight += addMe.Weight;
        addMe.ActionsOnAdd((IEquipment) this);
        flag = true;
      }
      return flag;
    }

    public virtual void RemoveOption(IEquipmentOption removeMe)
    {
      IEquipmentOption equipmentOption = this._options.Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => o.Id.Equals(removeMe.Id) && o.InstanceID.Equals(removeMe.InstanceID))).FirstOrDefault<IEquipmentOption>();
      if (equipmentOption == null)
        return;
      this._options.Remove(equipmentOption);
      this.Weight -= equipmentOption.Weight;
      equipmentOption.ActionsOnRemoval((IEquipment) this);
    }

    public override bool ModifiesSkillTask(string skillName, string stat)
    {
      bool flag = base.ModifiesSkillTask(skillName, stat);
      if (!flag)
        flag = this.Options.Any<IEquipmentOption>((Func<IEquipmentOption, bool>) (option => option.ModifiesSkillTask(skillName, stat)));
      return flag;
    }

    public override List<string> SkillTasksModified()
    {
      HashSet<string> first = new HashSet<string>();
      foreach (IEquipmentOption option in this.Options)
        first.UnionWith((IEnumerable<string>) option.SkillTasksModified());
      return first.Union<string>((IEnumerable<string>) base.SkillTasksModified()).ToList<string>();
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      int num = base.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
      foreach (IEquipmentOption option in this.Options)
      {
        if (option.ModifiesSkillTask(skillName, stat.Name ?? ""))
          num += option.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
      }
      return num;
    }
  }
}
