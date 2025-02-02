
// Type: com.digitalarcsystems.Traveller.DataModel.AutoQualOnStatValue




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class AutoQualOnStatValue : QualStatChallenge
  {
    [JsonConstructor]
    public AutoQualOnStatValue()
    {
    }

    public AutoQualOnStatValue(string stat, int thisValueOrAbove)
      : base(stat, thisValueOrAbove)
    {
      this.Description = "Qualify Automatically if  " + stat + " is " + thisValueOrAbove.ToString() + " or higher";
    }

    public AutoQualOnStatValue(string stat, int success, string description)
      : base(stat, success)
    {
      this.Description = description;
    }

    public override int percentChanceOfSuccess(Character character, int total_modifier)
    {
      return character.getAttributeValue(this.stat_name) >= this.success_on ? 100 : 0;
    }

    public override RollEffect qualify(Character character, int totalModifier)
    {
      return new RollEffect(this.qualificationRoll(character, totalModifier), character.getAttributeValue(this.stat_name));
    }

    public override RollParam qualificationRoll(Character forCharacter, int withModifier)
    {
      return new RollParam()
      {
        description = this.Description,
        rawMinSuccessValue = forCharacter.getAttributeValue(this.stat_name) >= this.success_on ? 2 : 15,
        isToBeAnimated = false
      };
    }
  }
}
