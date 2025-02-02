
// Type: com.digitalarcsystems.Traveller.DataModel.Events.StatBasedOutcome




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class StatBasedOutcome : Event
  {
    [JsonProperty]
    public Outcome primaryOutcome;
    [JsonProperty]
    public Outcome secondaryOutcome;
    [JsonProperty]
    public string primaryStat;
    [JsonProperty]
    private int level = 0;

    [JsonConstructor]
    public StatBasedOutcome()
    {
    }

    public StatBasedOutcome(string name, string description)
      : base(name, description)
    {
    }

    public StatBasedOutcome(
      string name,
      string description,
      string primary_stat_name,
      Outcome primaryOutcome,
      Outcome secondaryOutcome)
      : base(name, description)
    {
      this.primaryStat = primary_stat_name;
      this.primaryOutcome = primaryOutcome;
      this.secondaryOutcome = secondaryOutcome;
    }

    public StatBasedOutcome(
      string name,
      string description,
      string primary_stat_name,
      int minStatLevel,
      Outcome primaryOutcome,
      Outcome secondaryOutcome)
      : base(name, description)
    {
      this.primaryStat = primary_stat_name;
      this.primaryOutcome = primaryOutcome;
      this.secondaryOutcome = secondaryOutcome;
      this.level = minStatLevel;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      Attribute attribute = currentState.character.getAttribute(this.primaryStat);
      if (attribute != null && attribute.Value > this.level)
        this.primaryOutcome.handleOutcome(currentState);
      else
        this.secondaryOutcome.handleOutcome(currentState);
      base.handleOutcome(currentState);
    }
  }
}
