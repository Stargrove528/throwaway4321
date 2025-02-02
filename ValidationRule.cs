
// Type: com.digitalarcsystems.Traveller.utility.Validator.ValidationRule




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public abstract class ValidationRule : IValidationRule, IDescribable
  {
    public string Description
    {
      get => this.GetType().Name;
      set
      {
      }
    }

    public string Name
    {
      get => this.GetType().Name;
      set
      {
      }
    }

    protected abstract bool DoValidation(ValidationResult result, Character validateMe);

    public ValidationResult Validate(Character validateMe)
    {
      ValidationResult result = new ValidationResult()
      {
        Error = false,
        message = this.Name + ": "
      };
      if (this.DoValidation(result, validateMe))
        result.message += "OK";
      return result;
    }
  }
}
