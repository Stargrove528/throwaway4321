
// Type: com.digitalarcsystems.Traveller.DataModel.AttackData




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class AttackData
  {
    [JsonProperty]
    public RollEffect attackRoll;
    [JsonProperty]
    public int attack_aim_value;
    [JsonProperty]
    public int attack_action_modifier;
    [JsonProperty]
    public int attack_manual_modifier;
    [JsonProperty]
    public RollEffect damageRoll;

    [JsonConstructor]
    public AttackData()
    {
    }
  }
}
