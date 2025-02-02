
// Type: com.digitalarcsystems.Traveller.DataModel.Events.IfStatOverTarget




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class IfStatOverTarget : Outcome
  {
    [JsonProperty]
    private string targetStat = (string) null;
    [JsonProperty]
    private int threshold = 0;
    [JsonProperty]
    private Outcome[] overTargetOutcomes = new Outcome[0];
    [JsonProperty]
    private Outcome[] underTargetOutcomes = new Outcome[0];

    [JsonConstructor]
    public IfStatOverTarget(
      string stat,
      int target,
      Outcome[] overTargetInclusiveOutcomes,
      Outcome[] underTargetOutcomes)
    {
      if (overTargetInclusiveOutcomes != null)
        this.overTargetOutcomes = overTargetInclusiveOutcomes;
      if (underTargetOutcomes != null)
        this.underTargetOutcomes = underTargetOutcomes;
      this.threshold = target;
      this.targetStat = stat;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      if (currentState.character.getAttributeValue(this.targetStat) > this.threshold)
      {
        foreach (Outcome overTargetOutcome in this.overTargetOutcomes)
          overTargetOutcome.handleOutcome(currentState);
      }
      else
      {
        foreach (Outcome underTargetOutcome in this.underTargetOutcomes)
          underTargetOutcome.handleOutcome(currentState);
      }
    }
  }
}
