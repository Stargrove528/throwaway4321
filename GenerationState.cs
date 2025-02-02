
// Type: com.digitalarcsystems.Traveller.GenerationState




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class GenerationState : IAsset, IDescribable, IAssetBase, INonLicensedAsset
  {
    public string engineID;
    public Character character;
    [JsonIgnore]
    public GenerationState previousState;
    public CreationOperation nextOperation;
    public Career currentCareer;
    public List<Event.Injury> injuries = new List<Event.Injury>();
    public bool QueryUseOfAnagathics = true;
    public string nextCarrerMustBe = "";
    public int startNextCareerAtRank = 0;
    public bool endCurrentCareer = false;
    public bool mustContinueInCareer = false;
    public int careerAdvancementModifier = 0;
    public int careerSurvivalModifier = 0;
    [JsonIgnore]
    public ISkillSource skillSource;
    [JsonIgnore]
    public IPeopleSource peopleSource;
    [JsonIgnore]
    public IDecisionMaker decisionMaker;
    [JsonIgnore]
    public IBenefitRecorder recorder;
    [JsonIgnore]
    public IList<Career> draftCareers = (IList<Career>) null;
    [JsonIgnore]
    public ICareerSource careerSource = (ICareerSource) null;
    public Dictionary<string, int> qualificationModifiers = new Dictionary<string, int>();
    public List<Event.ActionsOnQualification> actionsOnQualifications = new List<Event.ActionsOnQualification>();
    public bool skipProcessAdvancementOrCommission = false;
    [JsonIgnore]
    public const string GENERIC_CAREER_KEY = "ANY";

    public virtual GenerationState createNextCharacterGenerationState(
      CreationOperation nextOperation)
    {
      GenerationState characterGenerationState = new GenerationState(DataManager.UserID)
      {
        engineID = this.engineID,
        nextOperation = nextOperation,
        character = this.character,
        previousState = this,
        QueryUseOfAnagathics = this.QueryUseOfAnagathics,
        currentCareer = this.currentCareer,
        skillSource = this.skillSource,
        mustContinueInCareer = this.mustContinueInCareer,
        endCurrentCareer = this.endCurrentCareer,
        nextCarrerMustBe = this.nextCarrerMustBe,
        peopleSource = this.peopleSource,
        decisionMaker = this.decisionMaker,
        recorder = this.recorder,
        draftCareers = this.draftCareers,
        injuries = this.injuries,
        careerSource = this.careerSource,
        skipProcessAdvancementOrCommission = this.skipProcessAdvancementOrCommission
      };
      if (this.qualificationModifiers.Count > 0)
      {
        foreach (string key in this.qualificationModifiers.Keys)
          characterGenerationState.qualificationModifiers[key] = this.qualificationModifiers[key];
      }
      if (!this.qualificationModifiers.ContainsKey("ANY"))
        characterGenerationState.qualificationModifiers["ANY"] = 0;
      if (this.actionsOnQualifications.Count > 0)
        characterGenerationState.actionsOnQualifications.AddRange((IEnumerable<Event.ActionsOnQualification>) this.actionsOnQualifications);
      if ((this.endCurrentCareer || nextOperation == CreationOperation.CHOOSE_CAREER || nextOperation == CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT) && !this.mustContinueInCareer)
      {
        characterGenerationState.careerSurvivalModifier = 0;
        characterGenerationState.careerAdvancementModifier = 0;
      }
      else
      {
        characterGenerationState.careerSurvivalModifier = this.careerSurvivalModifier;
        characterGenerationState.careerAdvancementModifier = this.careerAdvancementModifier;
      }
      return characterGenerationState;
    }

    public int getQualificationModifier() => this.qualificationModifiers["ANY"];

    public int getQualificationModifier(string careerName)
    {
      int qualificationModifier = this.qualificationModifiers["ANY"];
      if (this.qualificationModifiers.ContainsKey(careerName))
        qualificationModifier += this.qualificationModifiers[careerName];
      return qualificationModifier;
    }

    public void clearQualificationModifier() => this.qualificationModifiers["ANY"] = 0;

    public void clearQualificationModifier(string careerName)
    {
      this.clearQualificationModifier();
      if (!this.qualificationModifiers.ContainsKey(careerName))
        return;
      this.qualificationModifiers.Remove(careerName);
    }

    [JsonConstructor]
    public GenerationState(int creatingUser)
    {
      if (this.Id == Guid.Empty)
        this.Id = Guid.NewGuid();
      this.CreatingUser = creatingUser;
    }

    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public string DefaultFileName => this.Id.ToString("D") + ".tgsjson";

    public int CreatingUser { get; set; }
  }
}
