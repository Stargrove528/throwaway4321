
// Type: com.digitalarcsystems.Traveller.DataModel.Events.LoseMusteringOutBenefitsGainedThisTerm




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class LoseMusteringOutBenefitsGainedThisTerm : Event
  {
    [JsonConstructor]
    public LoseMusteringOutBenefitsGainedThisTerm()
    {
    }

    public LoseMusteringOutBenefitsGainedThisTerm(string name, string description)
      : base(name, description)
    {
    }

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.character.CurrentTerm.num_mustering_out_benefits_awarded_this_term = 0;
      base.handleOutcome(currentState);
    }
  }
}
