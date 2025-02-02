
// Type: com.digitalarcsystems.Traveller.DataModel.Events.QualifyForDifferentCareerAddingTermsAsBonus




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class QualifyForDifferentCareerAddingTermsAsBonus : Event
  {
    [JsonProperty]
    private Career targetCareer;

    [JsonConstructor]
    public QualifyForDifferentCareerAddingTermsAsBonus()
    {
    }

    public QualifyForDifferentCareerAddingTermsAsBonus(
      string name,
      string description,
      Career qualifyForMe)
      : base(name, description)
    {
      this.targetCareer = qualifyForMe;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      if (this.targetCareer.qualifyForCareer(currentState.character, currentState.character.CurrentTerm.numberOfTermsInCareerIncludingThisOne))
        new Outcome.NextCareerMustBe(this.targetCareer.Name, true).handleOutcome(currentState);
      base.handleOutcome(currentState);
    }
  }
}
