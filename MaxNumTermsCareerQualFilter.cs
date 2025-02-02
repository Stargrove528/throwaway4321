
// Type: com.digitalarcsystems.Traveller.DataModel.MaxNumTermsCareerQualFilter




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MaxNumTermsCareerQualFilter : CareerQualFilter
  {
    [JsonProperty]
    public int MaxNumOfTerms { get; set; }

    [JsonConstructor]
    public MaxNumTermsCareerQualFilter()
    {
    }

    public MaxNumTermsCareerQualFilter(int max_num_terms) => this.MaxNumOfTerms = max_num_terms;

    public override bool passFilter(Character character)
    {
      bool flag = false;
      if (character.TotalNumberOfTerms <= this.MaxNumOfTerms + 1)
        flag = true;
      return flag;
    }
  }
}
