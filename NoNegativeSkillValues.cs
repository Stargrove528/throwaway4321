
// Type: com.digitalarcsystems.Traveller.utility.Validator.NoNegativeSkillValues




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class NoNegativeSkillValues : ValidationRule
  {
    protected override bool DoValidation(ValidationResult result, Character validateMe)
    {
      List<ISkill> source = new List<ISkill>((IEnumerable<ISkill>) validateMe.Skills);
      if (source.Any<ISkill>((Func<ISkill, bool>) (s => s.Level < 0)))
      {
        result.Error = true;
        result.message += string.Join(", ", source.Where<ISkill>((Func<ISkill, bool>) (s => s.Level < 0)).Select<ISkill, string>((Func<ISkill, string>) (s => s.Name)).ToArray<string>());
        result.message += " have negative values";
      }
      return !result.Error;
    }
  }
}
