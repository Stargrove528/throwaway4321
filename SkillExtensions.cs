
// Type: com.digitalarcsystems.Traveller.DataModel.SkillExtensions




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public static class SkillExtensions
  {
    public static void AddCascade(this ISkill skill, ISkill cascade)
    {
      ISkill skill1 = cascade.Clone<ISkill>();
      SkillList specializationSkills = skill.SpecializationSkills;
      specializationSkills.Add(skill1);
      skill.SpecializationSkills = specializationSkills;
      cascade.SetParent(skill);
    }

    public static void SetParent(this ISkill skill, ISkill parent)
    {
      skill.Parent = parent.Clone<ISkill>();
    }
  }
}
