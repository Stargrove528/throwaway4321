
// Type: com.digitalarcsystems.Traveller.DataModel.Events.RerollStat




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class RerollStat : Event
  {
    [JsonProperty]
    public string stat { get; set; }

    [JsonConstructor]
    public RerollStat()
    {
    }

    public RerollStat(string statName) => this.stat = statName;

    public RerollStat(string name, string description)
      : base(name, description)
    {
    }

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.character.getAttribute(this.stat).Generate();
      base.handleOutcome(currentState);
    }
  }
}
