
// Type: com.digitalarcsystems.Traveller.DataModel.Attack




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Attack
  {
    [JsonProperty]
    public string skillNameUsed;
    [JsonProperty]
    public string weaponNameUsed;
    [JsonProperty]
    public List<AttackData> attackRolls = new List<AttackData>();

    [JsonConstructor]
    public Attack()
    {
    }
  }
}
