
// Type: com.digitalarcsystems.Traveller.DataModel.Initiative




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Initiative
  {
    [JsonProperty]
    public bool hastened;
    [JsonProperty]
    public RollEffect roll;
    [JsonProperty]
    public bool usedOwnTacticsSkill = false;
    [JsonProperty]
    public RollEffect ownTacticsRoll;
    [JsonProperty]
    public int tacticsValue = 0;
    [JsonProperty]
    public int tacticsEffect = 0;

    [JsonProperty]
    public int Effect { get; set; }

    [JsonConstructor]
    public Initiative()
    {
    }
  }
}
