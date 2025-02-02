
// Type: com.digitalarcsystems.Traveller.DataModel.Events.RestoreStat




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class RestoreStat : Event
  {
    [JsonProperty]
    public string StatName { get; set; }

    [JsonConstructor]
    public RestoreStat()
    {
    }

    public RestoreStat(string name, string description)
      : base(name, description)
    {
    }

    public RestoreStat(string stat_name) => this.StatName = stat_name;

    public override void handleOutcome(GenerationState currentState)
    {
      Attribute attribute = currentState.character.getAttribute(this.StatName);
      int rawRoll = attribute.RawRoll;
      int num = rawRoll != 0 ? rawRoll + attribute.RacialBonus : attribute.UninjuredValue;
      attribute.Value = num;
      base.handleOutcome(currentState);
    }
  }
}
