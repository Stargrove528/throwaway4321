
// Type: com.digitalarcsystems.Traveller.utility.Validator.StatsNotZeroVR




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class StatsNotZeroVR : ValidationRule
  {
    protected override bool DoValidation(ValidationResult result, Character validateMe)
    {
      IList<com.digitalarcsystems.Traveller.DataModel.Attribute> attributes = validateMe.getAttributes();
      if (attributes.Any<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (s => !s.Name.ToLowerInvariant().Contains("psi") && s.Value == 0)))
      {
        result.Error = true;
        result.message += string.Join(", ", attributes.Where<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (s => s.Value == 0)).Select<com.digitalarcsystems.Traveller.DataModel.Attribute, string>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, string>) (s => s.Name)).ToArray<string>());
        result.message += " has/have a ZERO VALUE.";
      }
      return !result.Error;
    }
  }
}
