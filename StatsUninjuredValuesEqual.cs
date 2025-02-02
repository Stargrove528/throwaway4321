
// Type: com.digitalarcsystems.Traveller.utility.Validator.StatsUninjuredValuesEqual




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class StatsUninjuredValuesEqual : ValidationRule
  {
    protected override bool DoValidation(ValidationResult result, Character validateMe)
    {
      IList<com.digitalarcsystems.Traveller.DataModel.Attribute> attributes = validateMe.getAttributes();
      if (attributes.Any<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (s => s.Value != s.UninjuredValue)))
      {
        result.Error = true;
        result.message += string.Join(", ", attributes.Where<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (s => s.Value != s.UninjuredValue)).Select<com.digitalarcsystems.Traveller.DataModel.Attribute, string>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, string>) (s => s.Name)).ToArray<string>());
        result.message += " have differring Value and Uninjured Value.";
      }
      return !result.Error;
    }
  }
}
