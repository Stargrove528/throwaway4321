
// Type: com.digitalarcsystems.Traveller.DataModel.PreviousCareerQualModifier




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class PreviousCareerQualModifier : CareerQualModifier
  {
    [JsonConstructor]
    public PreviousCareerQualModifier()
    {
    }

    public PreviousCareerQualModifier(int modifier)
    {
      this.modifier = modifier;
      this.Description = "A cumulative " + modifier.ToString() + " is added for each previous career.";
    }

    public override int getQualModifier(Character character)
    {
      return character.NumberOfCareers * this.modifier;
    }
  }
}
