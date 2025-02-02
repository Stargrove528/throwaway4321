
// Type: com.digitalarcsystems.Traveller.DataModel.SkillDictionary




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class SkillDictionary : Dictionary<string, ISkill>
  {
    public SkillDictionary()
    {
    }

    public SkillDictionary(Dictionary<string, Skill> skills)
    {
      foreach (KeyValuePair<string, Skill> skill in skills)
        this.Add(skill.Key, (ISkill) skill.Value);
    }

    public static implicit operator SkillDictionary(Dictionary<string, Skill> source)
    {
      SkillDictionary skillDictionary = new SkillDictionary();
      foreach (KeyValuePair<string, Skill> keyValuePair in source)
        skillDictionary.Add(keyValuePair.Key, (ISkill) keyValuePair.Value);
      return skillDictionary;
    }
  }
}
