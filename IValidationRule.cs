
// Type: com.digitalarcsystems.Traveller.utility.Validator.IValidationRule




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public interface IValidationRule : IDescribable
  {
    ValidationResult Validate(Character validateMe);
  }
}
