
// Type: com.digitalarcsystems.Traveller.DataModel.Events.GenderBasedOutcome




using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class GenderBasedOutcome : Event
  {
    [JsonProperty]
    protected Dictionary<string, Outcome> genderBasedOutcomes = new Dictionary<string, Outcome>();

    [JsonConstructor]
    public GenderBasedOutcome()
    {
    }

    public GenderBasedOutcome(string name, string description)
      : base(name, description)
    {
      this._name = name;
      this.Description = description;
    }

    public GenderBasedOutcome addGenderOutcome(string gender, Outcome outcome)
    {
      this.genderBasedOutcomes.Add(gender.ToLowerInvariant(), outcome);
      return this;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      string lowerInvariant = currentState.character.Gender.name.ToLowerInvariant();
      if (this.genderBasedOutcomes.ContainsKey(lowerInvariant))
        this.genderBasedOutcomes[lowerInvariant].handleOutcome(currentState);
      else
        EngineLog.Error("No Available GenderOutcome for [" + lowerInvariant + "].");
    }

    public override string ToString()
    {
      string source = this._name != null ? this._name : "";
      if (!source.Any<char>())
      {
        foreach (string key in this.genderBasedOutcomes.Keys)
        {
          if (source.Any<char>())
            source += ", ";
          source = source + key[0].ToString().ToUpper() + ": " + this.genderBasedOutcomes[key]?.ToString();
        }
      }
      return source;
    }
  }
}
