
// Type: com.digitalarcsystems.Traveller.DataModel.MusteringOutBenefit




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MusteringOutBenefit
  {
    public int roll = 2;
    public int cash = 2;
    [JsonProperty]
    public Outcome benefit = (Outcome) null;

    public MusteringOutBenefit()
    {
    }

    [JsonConstructor]
    public MusteringOutBenefit(int roll, int cash, Outcome benefit)
    {
      this.roll = roll;
      this.cash = cash;
      this.benefit = benefit;
    }
  }
}
