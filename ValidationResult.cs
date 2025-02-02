
// Type: com.digitalarcsystems.Traveller.ValidationResult




#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class ValidationResult
  {
    private ValidationResult.Type type = ValidationResult.Type.ERROR;
    private string message = (string) null;

    public virtual ValidationResult.Type getType() => this.type;

    public virtual string Message => this.type.ToString() + ": " + this.message;

    public static ValidationResult error(string msg)
    {
      return new ValidationResult(ValidationResult.Type.ERROR, msg);
    }

    public static ValidationResult warning(string msg)
    {
      return new ValidationResult(ValidationResult.Type.WARNING, msg);
    }

    public static ValidationResult inf(string msg)
    {
      return new ValidationResult(ValidationResult.Type.INFO, msg);
    }

    public ValidationResult(ValidationResult.Type type, string message)
    {
      this.type = type;
      this.message = message;
    }

    public virtual bool Error => this.type == ValidationResult.Type.ERROR;

    public enum Type
    {
      ERROR,
      WARNING,
      INFO,
    }
  }
}
