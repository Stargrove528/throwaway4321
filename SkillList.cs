
// Type: com.digitalarcsystems.Traveller.DataModel.SkillList




using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class SkillList : List<ISkill>
  {
    public SkillList()
    {
    }

    public SkillList(List<Skill> skills)
    {
      foreach (ISkill skill in skills)
        this.Add(skill);
    }

    public static implicit operator SkillList(List<Skill> source)
    {
      SkillList skillList = new SkillList();
      skillList.AddRange(source.Cast<ISkill>());
      return skillList;
    }
  }
}
