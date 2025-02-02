
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.SkillAugmentationByCategory




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class SkillAugmentationByCategory : Augmentation
  {
    public string CategoryName { get; set; }

    [JsonConstructor]
    public SkillAugmentationByCategory(List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
    }

    public SkillAugmentationByCategory()
    {
    }

    public SkillAugmentationByCategory(IAugmentation copyMe)
      : base(copyMe)
    {
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      int num = 0;
      ISkill skill = this.ObtainSkill(skillName);
      if (skill != null && skill.Categories.Any<string>((Func<string, bool>) (c => c.ToLowerInvariant().Equals(this.CategoryName.ToLowerInvariant()))))
        num = this.AugmentationBonus;
      return num;
    }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      return this.BonusProvided(skillName, 0, 0, (com.digitalarcsystems.Traveller.DataModel.Attribute) null) != 0;
    }

    private ISkill ObtainSkill(string skillName)
    {
      ISkill skill = DataManager.Instance.Skills.FirstOrDefault<ISkill>((Func<ISkill, bool>) (s => s.Name.ToLowerInvariant().Equals(skillName.ToLowerInvariant())));
      if (skill == null)
        EngineLog.Error("Unable to find skill [" + skillName + "] when looking for its categories.");
      return skill;
    }
  }
}
