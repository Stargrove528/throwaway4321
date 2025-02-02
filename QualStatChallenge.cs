
// Type: com.digitalarcsystems.Traveller.DataModel.QualStatChallenge




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class QualStatChallenge : CareerChallenge
  {
    public QualStatChallenge(string stat, int success)
    {
      this.stat_name = stat;
      this.success_on = success;
      this.Description = "Qualify with on " + stat + " " + success.ToString() + "+";
    }

    public QualStatChallenge(string stat, int success, string description)
      : this(stat, success)
    {
      this.Description = description;
    }

    [JsonConstructor]
    public QualStatChallenge()
    {
    }

    public override int percentChanceOfSuccess(Character character, int totalModifier)
    {
      Attribute attribute = this.getAttribute(character);
      return Dice.ProbabilityPercent(2, this.success_on - totalModifier - attribute.Modifier);
    }

    public override RollParam qualificationRoll(Character forCharacter, int withModifier)
    {
      return new RollParam(this.getAttribute(forCharacter), this.success_on, withModifier, this.Description);
    }

    private Attribute getAttribute(Character forCharacter)
    {
      Attribute attribute = forCharacter.getAttribute(this.stat_name);
      if (attribute == null)
      {
        attribute = new Attribute(this.stat_name, 6, 2, 0);
        attribute.InitializeValue(0);
      }
      return attribute;
    }
  }
}
