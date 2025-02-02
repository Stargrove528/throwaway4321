
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.SkillAugmentation




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class SkillAugmentation : Augmentation, IEmbeddedSkill
  {
    public int EmbeddedSkillLevel { get; set; }

    public override int AugmentationBonus
    {
      get => this.EmbeddedSkillLevel;
      set => this.EmbeddedSkillLevel = value;
    }

    public string EmbeddedSkillName { get; set; }

    public bool MustBeAlreadyPossessed { get; set; }

    [JsonConstructor]
    public SkillAugmentation(List<string> allowedUpgradeCategories)
    {
      this.AllowedUpgradeCategories = allowedUpgradeCategories;
    }

    public SkillAugmentation()
    {
    }

    public SkillAugmentation(IAugmentation copyMe)
      : base(copyMe)
    {
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      Attribute stat)
    {
      int num = 0;
      if (skillName.ToLower().Equals(this.EmbeddedSkillName.ToLower()))
        num = this.EmbeddedSkillLevel;
      return num;
    }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      return skillName.ToLower().Equals(this.EmbeddedSkillName.ToLower());
    }

    public override List<string> SkillTasksModified()
    {
      return new List<string>()
      {
        this.EmbeddedSkillName != null ? this.EmbeddedSkillName.ToLowerInvariant() : ""
      };
    }
  }
}
