
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Computer




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Computer : 
    UpgradableEquipment,
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
    private int _rating;
    [JsonProperty]
    private readonly List<Software> _runningSoftware = new List<Software>();

    private SpecializedComputer getSpecializedComputer()
    {
      return (SpecializedComputer) this.Options.Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => o is SpecializedComputer)).FirstOrDefault<IEquipmentOption>();
    }

    [JsonIgnore]
    public int Rating
    {
      get => this._rating + this.GetSpecializationBonusToRating();
      set => this._rating = value;
    }

    [JsonIgnore]
    public int AvailableRating
    {
      get
      {
        List<Software> runningSoftware = this.RunningSoftware;
        int rating = this.Rating;
        foreach (Software software in runningSoftware)
          rating -= software.CurrentRating;
        return rating;
      }
    }

    private int GetSpecializationBonusToRating()
    {
      int specializationBonusToRating = 0;
      SpecializedComputer specializedComputer = this.getSpecializedComputer();
      if (specializedComputer != null)
      {
        int rating = specializedComputer.Rating;
        IEnumerator<Software> enumerator = (IEnumerator<Software>) this.RunningSoftware.GetEnumerator();
        while (specializationBonusToRating < rating && enumerator.MoveNext())
        {
          Software current = enumerator.Current;
          int val1 = specializedComputer.BonusProvided(current);
          if (val1 > 0)
            specializationBonusToRating += Math.Min(val1, rating - specializationBonusToRating);
        }
      }
      return specializationBonusToRating;
    }

    [JsonConstructor]
    public Computer()
    {
    }

    public Computer(IComputer copyMe)
      : base((IUpgradable) copyMe)
    {
      this.Rating = copyMe.Rating;
      copyMe.RunningSoftware.ForEach((Action<Software>) (s => this.LoadSoftware(s)));
    }

    public Computer(IEquipment copyMe)
      : base(copyMe)
    {
    }

    [JsonIgnore]
    public List<Software> RunningSoftware
    {
      get => new List<Software>((IEnumerable<Software>) this._runningSoftware);
    }

    public bool CanRun(Software canIRun)
    {
      int num = 0;
      SpecializedComputer specializedComputer = this.getSpecializedComputer();
      if (specializedComputer != null)
      {
        int rating = specializedComputer.BonusProvided(canIRun) > 0 ? specializedComputer.Rating : 0;
        if (rating > 0)
          num = rating - this.GetSpecializationBonusToRating();
      }
      return canIRun.CurrentRating <= this.AvailableRating + num;
    }

    public override bool AddOption(IEquipmentOption addMe)
    {
      if (!(addMe is SpecializedComputer) || !this.Options.Any<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => o is SpecializedComputer)))
        return base.AddOption(addMe);
      this.RemoveOption((IEquipmentOption) (this.Options.Where<IEquipmentOption>((Func<IEquipmentOption, bool>) (o => o is SpecializedComputer)).First<IEquipmentOption>() as SpecializedComputer));
      return base.AddOption(addMe);
    }

    public bool LoadSoftware(Software runMe)
    {
      if (!this.CanRun(runMe))
        return false;
      this._runningSoftware.Add(runMe);
      return true;
    }

    public void UnloadSoftware(Software unloadMe)
    {
      if (!this._runningSoftware.Contains(unloadMe))
        return;
      this._runningSoftware.Remove(unloadMe);
    }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      int canonicalOrdinalForStat = com.digitalarcsystems.Traveller.DataModel.Attribute.GetCanonicalOrdinalForStat(statName);
      return this.GetBonusProvidedFromSubComponents(skillName, 0, -3, new com.digitalarcsystems.Traveller.DataModel.Attribute(statName, canonicalOrdinalForStat)) != 0;
    }

    protected int GetBonusProvidedFromSubComponents(
      string targetName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      int num1 = -100;
      foreach (Software software in this._runningSoftware)
      {
        if (software.ModifiesSkillTask(targetName, stat.Name))
        {
          int num2 = software.BonusProvided(targetName, difficulty, charactersSkillLevel, stat);
          if (num2 > num1)
            num1 = num2;
        }
      }
      if (num1 == -100)
        num1 = 0;
      return num1 + base.BonusProvided(targetName, difficulty, charactersSkillLevel, stat);
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      return this.GetBonusProvidedFromSubComponents(skillName, difficulty, charactersSkillLevel, stat);
    }

    public override List<string> SkillTasksModified()
    {
      HashSet<string> source = new HashSet<string>();
      foreach (Software software in this._runningSoftware)
        source.UnionWith((IEnumerable<string>) software.SkillTasksModified());
      foreach (ComputerOption option in this.Options)
        source.UnionWith((IEnumerable<string>) option.SkillTasksModified());
      return source.ToList<string>();
    }

    public int MaxRunnableRating(Software whatRatingCanIRunAt)
    {
      Software canIRun = new Software(whatRatingCanIRunAt);
      canIRun.Rating = canIRun.MaxRating;
      while (canIRun.CurrentRating > 0 && !this.CanRun(canIRun))
        --canIRun.CurrentRating;
      return canIRun.CurrentRating;
    }
  }
}
