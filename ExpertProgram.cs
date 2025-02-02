
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ExpertProgram




using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ExpertProgram : Software, IEmbeddedSkill
  {
    public ExpertProgram()
    {
    }

    public ExpertProgram(Software copyMe)
      : base(copyMe)
    {
    }

    public string EmbeddedSkillName { get; set; }

    public int EmbeddedSkillLevel
    {
      get => this.CurrentRating;
      set
      {
      }
    }

    public bool MustBeAlreadyPossessed => false;

    public override bool ModifiesSkillTask(string skillName, string statName)
    {
      if (skillName.Equals(this.EmbeddedSkillName, StringComparison.InvariantCultureIgnoreCase))
        return true;
      foreach (ISkill relatedSkill in this.getRelatedSkills())
      {
        if (skillName.Equals(relatedSkill.Name, StringComparison.InvariantCultureIgnoreCase))
          return true;
      }
      return false;
    }

    public override int BonusProvided(
      string skillName,
      int difficulty,
      int charactersSkillLevel,
      com.digitalarcsystems.Traveller.DataModel.Attribute stat)
    {
      int num = 0;
      if (this.ModifiesSkillTask(skillName, "") && difficulty <= 2 * this.CurrentRating + 8)
      {
        if (skillName.Equals(this.EmbeddedSkillName, StringComparison.InvariantCultureIgnoreCase))
        {
          num = Math.Max(this.CurrentRating - 1, 0);
          if (num > charactersSkillLevel)
            num += -1 * charactersSkillLevel;
          if (num <= charactersSkillLevel)
            num = 1;
        }
        else if (charactersSkillLevel < 0)
          num = -1 * charactersSkillLevel;
      }
      return num;
    }

    private List<ISkill> getRelatedSkills()
    {
      List<ISkill> relatedSkills = new List<ISkill>();
      ISkill skill = DataManager.Instance.GetAsset<ISkill>(this.EmbeddedSkillName).FirstOrDefault<ISkill>();
      if (skill != null)
      {
        if (skill.Parent != null)
        {
          ISkill asset = DataManager.Instance.GetAsset<ISkill>(DataManager.Instance.GetAsset<ISkill>(skill.Id).Parent.Id);
          relatedSkills.Add(asset);
          foreach (ISkill specializationSkill in (List<ISkill>) asset.SpecializationSkills)
          {
            if (specializationSkill.Name != skill.Name)
              relatedSkills.Add(specializationSkill);
          }
        }
      }
      else
        EngineLog.Warning("Looking for related skill to [" + this.EmbeddedSkillName + "] but couldn't find [" + this.EmbeddedSkillName + "]");
      return relatedSkills;
    }

    public override List<string> SkillTasksModified()
    {
      return new List<string>() { this.EmbeddedSkillName };
    }
  }
}
