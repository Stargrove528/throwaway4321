
// Type: com.digitalarcsystems.Traveller.utility.Validator.JoATIsRight




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class JoATIsRight : ValidationRule
  {
    public const string joat = "Jack-of-All-Trades";
    public const string joatlc = "jack-of-all-trades";

    protected override bool DoValidation(ValidationResult result, Character validateMe)
    {
      if (!validateMe.Skills.Any<ISkill>((Func<ISkill, bool>) (s => s.Name.ToLowerInvariant().Contains("jack-of-all-trades"))))
        ;
      return false;
    }
  }
}
