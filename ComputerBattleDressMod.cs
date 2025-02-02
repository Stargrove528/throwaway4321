
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ComputerBattleDressMod




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ComputerBattleDressMod : 
    BattleDressMod,
    IComputer,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable,
    IUpgradable
  {
    [JsonProperty]
    protected IComputer _computer;

    [JsonConstructor]
    public ComputerBattleDressMod()
    {
    }

    public ComputerBattleDressMod(BattleDressMod copyMe, IComputer computer)
      : base(copyMe)
    {
      this._computer = computer;
    }

    [JsonIgnore]
    public int Rating
    {
      get => this._computer.Rating;
      set => this._computer.Rating = value;
    }

    [JsonIgnore]
    public int AvailableRating => this._computer.AvailableRating;

    [JsonIgnore]
    public List<Software> RunningSoftware => this._computer.RunningSoftware;

    [JsonIgnore]
    public List<string> AllowedUpgradeCategories => this._computer.AllowedUpgradeCategories;

    [JsonIgnore]
    public List<IEquipmentOption> Options => this._computer.Options;

    [JsonIgnore]
    public List<IWeapon> WeaponSubcomponents => this._computer.WeaponSubcomponents;

    [JsonIgnore]
    public List<IComputer> ComputerSubcomponents => this._computer.ComputerSubcomponents;

    public bool AddOption(IEquipmentOption addMe) => this._computer.AddOption(addMe);

    public bool CanRun(Software canIRun) => this._computer.CanRun(canIRun);

    public bool LoadSoftware(Software runMe) => this._computer.LoadSoftware(runMe);

    public int MaxRunnableRating(Software whatRatingCanIRunAt)
    {
      return this._computer.MaxRunnableRating(whatRatingCanIRunAt);
    }

    public void RemoveOption(IEquipmentOption removeMe) => this._computer.RemoveOption(removeMe);

    public void UnloadSoftware(Software unloadMe) => this._computer.UnloadSoftware(unloadMe);

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      return this._computer.ModifiesSkillTask(skillName, statName);
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      Attribute stat)
    {
      return this._computer.BonusProvided(skillName, difficulty, charactersSkillLevel, stat);
    }
  }
}
