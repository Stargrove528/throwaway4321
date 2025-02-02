
// Type: com.digitalarcsystems.Traveller.ISkillSource




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface ISkillSource
  {
    ISkill getBasicTrainingSkill(string skillName);

    ISkill getSkill(string skillName);

    IList<ISkill> Skills { get; }

    Talent getBasicTrainingTalent(string talentName);

    Talent getTalent(string talentName);

    IList<Talent> Talents { get; }

    IList<Talent> getTalentsForTrait(Trait trait);

    List<Talent> getTalentsForTrait(string traitName);
  }
}
