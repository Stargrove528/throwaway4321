
// Type: com.digitalarcsystems.Traveller.DataModel.Combat




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Combat : Describable
  {
    [JsonProperty]
    public List<CombatRound> rounds = new List<CombatRound>();
    [JsonProperty]
    public GameDate gameDate = (GameDate) null;

    [JsonConstructor]
    public Combat()
    {
    }
  }
}
