
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.UpgradeWeapon




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class UpgradeWeapon : 
    IWeapon,
    IUpgradable,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IEquipmentOption,
    IEquipmentModifier,
    IModifier<IEquipment>,
    ILicensedAsset
  {
    [JsonProperty]
    protected IEquipmentOption _equipmentOption;
    [JsonProperty]
    protected IWeapon _weapon;

    [JsonConstructor]
    protected UpgradeWeapon()
    {
    }

    [JsonProperty]
    public Guid InstanceID { get; set; }

    public UpgradeWeapon(IWeapon weapon, IEquipmentOption option)
    {
      this._weapon = weapon;
      this._equipmentOption = option;
    }

    [JsonIgnore]
    public List<string> AllowedUpgradeCategories => this._weapon.AllowedUpgradeCategories;

    [JsonIgnore]
    public List<string> Categories => this._weapon.Categories;

    [JsonIgnore]
    public Dictionary<Guid, AssetMetadata> ChildAssets
    {
      get => this._weapon.ChildAssets;
      set => this._weapon.ChildAssets = value;
    }

    [JsonIgnore]
    public List<IComputer> ComputerSubcomponents => this._weapon.ComputerSubcomponents;

    [JsonIgnore]
    public int Cost
    {
      get => this._weapon.Cost;
      set => this._weapon.Cost = value;
    }

    [JsonIgnore]
    public int Damage
    {
      get => this._weapon.Damage;
      set => this._weapon.Damage = value;
    }

    [JsonIgnore]
    public int DamageExponentiator
    {
      get => this._weapon.DamageExponentiator;
      set => this._weapon.DamageExponentiator = value;
    }

    [JsonIgnore]
    public int DamageModifier
    {
      get => this._weapon.DamageModifier;
      set => this._weapon.DamageModifier = value;
    }

    [JsonIgnore]
    public int DamageMultiplier => this._weapon.DamageMultiplier;

    [JsonIgnore]
    public string Description
    {
      get => this._weapon.Description;
      set => this._weapon.Description = value;
    }

    [JsonIgnore]
    public string Name
    {
      get => this._weapon.Name;
      set => this._weapon.Name = value;
    }

    [JsonIgnore]
    public List<IEquipmentOption> Options => this._weapon.Options;

    [JsonIgnore]
    public string PrimaryAttribute
    {
      get => this._weapon.PrimaryAttribute;
      set => this._weapon.PrimaryAttribute = value;
    }

    [JsonIgnore]
    public PersonalWeaponRange Range
    {
      get => this._weapon.Range;
      set => this._weapon.Range = value;
    }

    [JsonIgnore]
    public string Skill
    {
      get => this._weapon.Skill;
      set => this._weapon.Skill = value;
    }

    [JsonIgnore]
    public List<string> StatsModifyingDamage => this._weapon.StatsModifyingDamage;

    [JsonIgnore]
    public string SubSkill
    {
      get => this._weapon.SubSkill;
      set => this._weapon.SubSkill = value;
    }

    [JsonIgnore]
    public List<AssetTag> Tags
    {
      get => this._weapon.Tags;
      set => this._weapon.Tags = value;
    }

    [JsonIgnore]
    public int TechLevel
    {
      get => this._weapon.TechLevel;
      set => this._weapon.TechLevel = value;
    }

    [JsonIgnore]
    public string Traits
    {
      get => this._weapon.Traits;
      set => this._weapon.Traits = value;
    }

    [JsonIgnore]
    public List<IWeapon> WeaponSubcomponents => this._weapon.WeaponSubcomponents;

    [JsonIgnore]
    public double Weight
    {
      get => this._weapon.Weight;
      set => this._weapon.Weight = value;
    }

    [JsonIgnore]
    string IDescribable.Description
    {
      get => this._weapon.Description;
      set => this._weapon.Description = value;
    }

    [JsonIgnore]
    public Guid Id
    {
      get => this._equipmentOption.Id;
      set => this._equipmentOption.Id = value;
    }

    [JsonIgnore]
    public string OriginalName => this._weapon.OriginalName;

    [JsonIgnore]
    public string OriginalDescription => this._weapon.OriginalDescription;

    [JsonIgnore]
    public List<string> UpgradeCategories
    {
      get => this._equipmentOption.UpgradeCategories;
      set => this._equipmentOption.UpgradeCategories = value;
    }

    public void ActionsOnAdd(IEquipment addedToThis)
    {
      this._equipmentOption.ActionsOnAdd(addedToThis);
    }

    public void ActionsOnRemoval(IEquipment removedFromThis)
    {
      this._equipmentOption.ActionsOnRemoval(removedFromThis);
    }

    public bool AddOption(IEquipmentOption addMe) => this._weapon.AddOption(addMe);

    public void AddTaskToModify(string skillName, int maxDifficulty, int characterSkillLevel)
    {
      this._weapon.AddTaskToModify(skillName, maxDifficulty, characterSkillLevel);
    }

    public int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      return this._weapon.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
    }

    public int CalculatePrice(IEquipment equipmentToBeAddedTo)
    {
      return this._equipmentOption.CalculatePrice(equipmentToBeAddedTo);
    }

    public bool ModifiesSkillTask(string skillName, string statName)
    {
      return this._weapon.ModifiesSkillTask(skillName, statName);
    }

    public void RemoveOption(IEquipmentOption removeMe) => this._weapon.RemoveOption(removeMe);

    public List<string> SkillTasksModified() => this._weapon.SkillTasksModified();

    public bool UseEquipmentSkill(
      string skillName,
      int difficulty,
      int characterSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      return this._weapon.UseEquipmentSkill(skillName, difficulty, characterSkillLevel, stat);
    }

    public void AddEquipmentAction(IEquipmentAction addMe)
    {
      this._equipmentOption.AddEquipmentAction(addMe);
    }

    public void AddTaskToModify(string stat, int bonus)
    {
      this._weapon.AddTaskToModify(stat, bonus);
    }
  }
}
