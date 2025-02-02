
// Type: com.digitalarcsystems.Traveller.RollEffect




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class RollEffect
  {
    [JsonProperty]
    public readonly RollEffect.Result result;
    [JsonProperty]
    public readonly int effect;
    [JsonProperty]
    public readonly int rawResult;
    [JsonProperty]
    public readonly bool isSuccessful;
    [JsonProperty]
    public readonly RollParam setting;

    [JsonIgnore]
    public int totalResult => this.rawResult + this.setting.totalModifier;

    [JsonConstructor]
    private RollEffect()
    {
    }

    public RollEffect(RollParam requirements, int rolledNumber)
    {
      if (requirements.totalModifier < -1000)
        Console.WriteLine("Total Modifier is ridiculous");
      this.setting = requirements;
      this.rawResult = rolledNumber;
      this.effect = rolledNumber - requirements.rawMinSuccessValue + requirements.totalModifier;
      this.isSuccessful = this.effect >= 0;
      if (this.effect < -6)
        this.result = RollEffect.Result.DISASTER;
      else if (this.effect < -3)
        this.result = RollEffect.Result.FAILURE;
      else if (this.effect < 0)
        this.result = RollEffect.Result.ALMOST;
      else if (this.effect < 2)
        this.result = RollEffect.Result.BARELY;
      else if (this.effect < 7)
        this.result = RollEffect.Result.SUCCESS;
      else
        this.result = RollEffect.Result.MIRACLE;
    }

    public enum Result
    {
      DISASTER,
      FAILURE,
      ALMOST,
      BARELY,
      SUCCESS,
      MIRACLE,
    }
  }
}
