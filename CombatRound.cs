
// Type: com.digitalarcsystems.Traveller.DataModel.CombatRound




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class CombatRound : Describable
  {
    [JsonProperty]
    public int number;
    [JsonProperty]
    public int saved_aim = 0;
    [JsonProperty]
    public int action_modifier = 0;
    [JsonProperty]
    public Initiative initiative = (Initiative) null;
    [JsonProperty]
    public CombatStance stance = CombatStance.Unknown;
    [JsonProperty]
    public int actionsRemaining = 3;
    [JsonProperty]
    public List<CombatActions> selectedActions = new List<CombatActions>();

    [JsonConstructor]
    public CombatRound()
    {
    }
  }
}
