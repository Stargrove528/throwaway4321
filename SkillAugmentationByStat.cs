
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.SkillAugmentationByStat




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class SkillAugmentationByStat : Augmentation
  {
    public string StatName { get; set; }

    [JsonConstructor]
    public SkillAugmentationByStat(List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
    }

    public SkillAugmentationByStat()
    {
    }

    public SkillAugmentationByStat(IAugmentation copyMe)
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
      if (stat.Name.ToLowerInvariant().Equals(this.StatName.ToLowerInvariant()))
        num = this.AugmentationBonus;
      return num;
    }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      return statName.ToLower().Equals(this.StatName.ToLower());
    }
  }
}
