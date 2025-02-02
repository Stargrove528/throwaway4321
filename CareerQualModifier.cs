
// Type: com.digitalarcsystems.Traveller.DataModel.CareerQualModifier




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class CareerQualModifier : Describable
  {
    [JsonProperty]
    private int _modifier = -1;

    [JsonProperty]
    public virtual int age_limit { get; set; }

    [JsonProperty]
    public virtual int modifier
    {
      get => this._modifier;
      set => this._modifier = value;
    }

    public virtual int getQualModifier(Character character) => int.MinValue;
  }
}
