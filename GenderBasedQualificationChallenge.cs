
// Type: com.digitalarcsystems.Traveller.DataModel.GenderBasedQualificationChallenge




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class GenderBasedQualificationChallenge : CareerChallenge
  {
    [JsonProperty]
    private Dictionary<string, CareerChallenge> genderChallenges;

    public GenderBasedQualificationChallenge()
    {
      this.genderChallenges = new Dictionary<string, CareerChallenge>();
    }

    [JsonConstructor]
    public GenderBasedQualificationChallenge(
      Dictionary<string, CareerChallenge> genderChallenges)
    {
      this.genderChallenges = genderChallenges;
    }

    protected CareerChallenge getApplicableChallenge(Character character)
    {
      return this.genderChallenges[character.Gender.name.ToLowerInvariant()];
    }

    public override int percentChanceOfSuccess(Character character, int totalModifier)
    {
      return this.getApplicableChallenge(character).percentChanceOfSuccess(character, totalModifier);
    }

    public override RollParam qualificationRoll(Character forCharacter, int withModifier)
    {
      return this.getApplicableChallenge(forCharacter).qualificationRoll(forCharacter, withModifier);
    }

    public GenderBasedQualificationChallenge AddGenderChallenge(
      string gender,
      string stat,
      int success_target)
    {
      this.genderChallenges.Add(gender.ToLowerInvariant(), (CareerChallenge) new QualStatChallenge(stat, success_target));
      return this;
    }

    public GenderBasedQualificationChallenge AddGenderChallenge(
      string gender,
      CareerChallenge challenge)
    {
      this.genderChallenges.Add(gender.ToLowerInvariant(), challenge);
      return this;
    }
  }
}
