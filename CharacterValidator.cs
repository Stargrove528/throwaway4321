
// Type: com.digitalarcsystems.Traveller.utility.Validator.CharacterValidator




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility.Validator
{
  public class CharacterValidator
  {
    private List<IValidationRule> rules = new List<IValidationRule>();

    public CharacterValidator()
    {
    }

    public CharacterValidator(ICollection<IValidationRule> vRules)
    {
      this.rules.AddRange((IEnumerable<IValidationRule>) vRules);
    }

    public void addRule(IValidationRule addMe) => this.rules.Add(addMe);

    public void addRules(ICollection<IValidationRule> addUs)
    {
      this.rules.AddRange((IEnumerable<IValidationRule>) addUs);
    }

    public void removeRule(IValidationRule removeMe) => this.rules.Remove(removeMe);

    public void removeRules(ICollection<IValidationRule> removeUs)
    {
      foreach (IValidationRule removeU in (IEnumerable<IValidationRule>) removeUs)
        this.rules.Remove(removeU);
    }

    public void clearRules() => this.rules.Clear();

    public ValidationResults Validate(Character validateMe)
    {
      ValidationResults validationResults = new ValidationResults();
      foreach (IValidationRule rule in this.rules)
      {
        ValidationResult validationResult = rule.Validate(validateMe);
        validationResults.Add(validationResult);
        if (validationResult.Error)
          validationResults.Success = false;
      }
      return validationResults;
    }
  }
}
