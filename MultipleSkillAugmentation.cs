
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.MultipleSkillAugmentation




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class MultipleSkillAugmentation : Augmentation
  {
    public List<string> SkillNames { get; set; }

    [JsonConstructor]
    public MultipleSkillAugmentation(List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
    }

    public MultipleSkillAugmentation()
    {
    }

    public MultipleSkillAugmentation(IAugmentation copyMe)
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
      if (this.SkillNames.Any<string>((Func<string, bool>) (sn => sn.ToLowerInvariant().Equals(skillName.ToLowerInvariant()))))
        num = this.AugmentationBonus;
      return num;
    }

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      return this.BonusProvided(skillName, 0, 0, (com.digitalarcsystems.Traveller.DataModel.Attribute) null) != 0;
    }

    public override List<string> SkillTasksModified()
    {
      return new List<string>(this.SkillNames.Select<string, string>((Func<string, string>) (s => s.ToLowerInvariant())));
    }
  }
}
