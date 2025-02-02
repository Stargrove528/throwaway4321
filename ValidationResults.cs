
// Type: com.digitalarcsystems.Traveller.utility.Validator.ValidationResults




using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class ValidationResults : List<ValidationResult>
  {
    public bool Success = true;

    public List<ValidationResult> getErrors()
    {
      return this.Where<ValidationResult>((Func<ValidationResult, bool>) (vr => vr.Error)).ToList<ValidationResult>();
    }
  }
}
