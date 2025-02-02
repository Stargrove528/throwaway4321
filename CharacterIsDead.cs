
// Type: com.digitalarcsystems.Traveller.utility.Validator.CharacterIsDead




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class CharacterIsDead : ValidationRule
  {
    protected override bool DoValidation(ValidationResult result, Character validateMe)
    {
      if (validateMe.hasDied())
      {
        result.Error = true;
        ValidationResult validationResult = result;
        validationResult.message = validationResult.message + "[" + validateMe.Id.ToString() + "] character has died.";
      }
      return !result.Error;
    }
  }
}
