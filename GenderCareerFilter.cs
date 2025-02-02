
// Type: com.digitalarcsystems.Traveller.DataModel.GenderCareerFilter




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class GenderCareerFilter : CareerQualFilter
  {
    [JsonProperty]
    protected string requiredGender;

    [JsonConstructor]
    public GenderCareerFilter()
    {
    }

    public GenderCareerFilter(Gender filterOutAllGendersButThisOne)
    {
      this.requiredGender = filterOutAllGendersButThisOne.name;
    }

    public GenderCareerFilter(string filterOutAllGendersButThisOne)
    {
      this.requiredGender = filterOutAllGendersButThisOne;
    }

    public override bool passFilter(Character character)
    {
      bool flag = false;
      if (character.Gender.name.ToLowerInvariant().Equals(this.requiredGender.ToLowerInvariant()))
        flag = true;
      return flag;
    }
  }
}
