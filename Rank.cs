
// Type: com.digitalarcsystems.Traveller.DataModel.Rank




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Rank
  {
    public int level;
    [JsonProperty]
    public List<Outcome> benefits;
    public string title = "";

    [JsonConstructor]
    public Rank()
    {
    }

    public Rank(int rLevel, string title)
    {
      this.level = rLevel;
      this.title = title;
    }

    public Rank(int rLevel, params Outcome[] rankBenefits)
    {
      this.level = rLevel;
      this.benefits = new List<Outcome>((IEnumerable<Outcome>) rankBenefits);
    }

    public Rank(int rLevel, string title, params Outcome[] rankBenefits)
      : this(rLevel, rankBenefits)
    {
      this.title = title;
    }
  }
}
