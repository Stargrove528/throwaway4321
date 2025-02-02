
// Type: com.digitalarcsystems.Traveller.DataModel.Career




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Career : IAsset, IDescribable, IAssetBase, ILicensedAsset
  {
    public const string advancementString = "Nah, I work for a living";
    public const string commissionString = "Yes, I belong in command";
    public const string drifterString = "Drifter";
    public const string draftString = "Draft";
    [JsonProperty]
    private List<Trait> requiredTraits = new List<Trait>();
    [JsonProperty]
    private bool useSpecialistSkillListForBasicTraining_Renamed;
    [JsonProperty]
    private bool hasAdvancedEducation;
    [JsonProperty]
    private List<Career.Specialization> specializations = new List<Career.Specialization>();
    [JsonProperty]
    private Outcome[] serviceSkills = new Outcome[6];
    [JsonProperty]
    private Outcome[] personalDevelopment = new Outcome[6];
    [JsonProperty]
    private Outcome[] officerSkills = new Outcome[6];
    [JsonProperty]
    private Outcome[] psionicSkills = new Outcome[6];
    [JsonProperty]
    private Outcome[] advancedEducation = new Outcome[6];
    [JsonProperty]
    private Dictionary<int, Rank> commisionedRanks = new Dictionary<int, Rank>();
    [JsonProperty]
    private Dictionary<int, Rank> careerRanks = new Dictionary<int, Rank>();
    [JsonProperty]
    private Dictionary<int, MusteringOutBenefit> musteringOutBenefits = new Dictionary<int, MusteringOutBenefit>();
    [JsonProperty]
    private List<CareerChallenge> qualChallenges = new List<CareerChallenge>();
    [JsonProperty]
    private List<CareerQualModifier> qualModifiers = new List<CareerQualModifier>();
    [JsonProperty]
    private List<CareerQualFilter> qualFilters = new List<CareerQualFilter>();
    [JsonIgnore]
    protected string _category = "";
    [JsonIgnore]
    protected RollEffect currentAdvancementRollEffect = (RollEffect) null;

    public static Career makeCareerStub(string careerName, params string[] specializations)
    {
      List<Career.Specialization> specializationList = new List<Career.Specialization>();
      foreach (string specialization in specializations)
        specializationList.Add(new Career.Specialization()
        {
          Name = specialization
        });
      return new Career()
      {
        Name = careerName,
        specializations = specializationList
      };
    }

    public string FlavorText { get; private set; }

    [JsonProperty]
    public bool CareerFinishedOnFailedAssignmentChange { get; set; }

    [JsonProperty]
    public bool AllowUseOfAnagathics { get; set; }

    [JsonProperty]
    public bool AutomaticCommissionIfStatIsAtOrOverTarget { get; set; } = false;

    [JsonProperty]
    public virtual string Name { get; set; }

    public virtual string Category
    {
      get => this._category == null ? "" : this._category;
      protected set => this._category = value;
    }

    public void addQualModifier(CareerQualModifier addMe)
    {
      if (addMe == null)
        return;
      this.qualModifiers.Add(addMe);
    }

    public void addQualFilter(CareerQualFilter addMe)
    {
      if (addMe == null)
        return;
      this.qualFilters.Add(addMe);
    }

    public void addQualChallenges(CareerChallenge addMe)
    {
      if (addMe == null)
        return;
      this.qualChallenges.Add(addMe);
    }

    public Career()
    {
      this.CareerFinishedOnFailedAssignmentChange = false;
      this.AdvancedEducationLevelRequired = 8;
      this.PreviousCareerDM = -1;
      this.Mishaps = new Dictionary<int, Event>();
      this.Events = new Dictionary<int, Event>();
      this.Commission = 8;
      this.Enlistment = 7;
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
      this.AllowUseOfAnagathics = true;
    }

    public Career(Career copyMe)
    {
      this.Name = copyMe.Name;
      this.Id = copyMe.Id;
      this.AdvancedEducationLevelRequired = copyMe.AdvancedEducationLevelRequired;
      this.Category = copyMe.Category;
      this.CommissionStat = copyMe.CommissionStat;
      this.Commission = copyMe.Commission;
      this.DraftCareer = copyMe.DraftCareer;
      this.FlavorText = copyMe.FlavorText;
      this.requiredTraits = copyMe.requiredTraits.Clone<List<Trait>>();
      this.Enlistment = copyMe.Enlistment;
      this.useSpecialistSkillListForBasicTraining_Renamed = copyMe.useSpecialistSkillListForBasicTraining_Renamed;
      this.hasAdvancedEducation = copyMe.hasAdvancedEducation;
      this.specializations = copyMe.specializations.Clone<List<Career.Specialization>>();
      this.serviceSkills = copyMe.serviceSkills.Clone<Outcome[]>();
      this.personalDevelopment = copyMe.personalDevelopment.Clone<Outcome[]>();
      if (copyMe.officerSkills != null && copyMe.officerSkills.Length != 0)
        this.officerSkills = copyMe.officerSkills.Clone<Outcome[]>();
      if (copyMe.advancedEducation != null && copyMe.advancedEducation.Length != 0)
        this.advancedEducation = copyMe.advancedEducation.Clone<Outcome[]>();
      this.Events = copyMe.Events.Clone<Dictionary<int, Event>>();
      this.Mishaps = copyMe.Mishaps.Clone<Dictionary<int, Event>>();
      if (copyMe.hasCommissions())
        this.commisionedRanks = copyMe.commisionedRanks.Clone<Dictionary<int, Rank>>();
      if (copyMe.careerRanks != null && copyMe.careerRanks.Count > 0)
        this.careerRanks = copyMe.careerRanks.Clone<Dictionary<int, Rank>>();
      this.musteringOutBenefits = copyMe.musteringOutBenefits.Clone<Dictionary<int, MusteringOutBenefit>>();
      this.Description = copyMe.Description;
      this.qualChallenges = copyMe.qualChallenges.Clone<List<CareerChallenge>>();
      this.qualModifiers = copyMe.qualModifiers.Clone<List<CareerQualModifier>>();
      this.qualFilters = copyMe.qualFilters.Clone<List<CareerQualFilter>>();
      this.CareerFinishedOnFailedAssignmentChange = copyMe.CareerFinishedOnFailedAssignmentChange;
      this.AllowUseOfAnagathics = copyMe.AllowUseOfAnagathics;
      this.AutomaticCommissionIfStatIsAtOrOverTarget = copyMe.AutomaticCommissionIfStatIsAtOrOverTarget;
    }

    [JsonConstructor]
    public Career(string name, string description, string category)
    {
      this.AdvancedEducationLevelRequired = 8;
      this.Name = name;
      this.PreviousCareerDM = -1;
      this.Mishaps = new Dictionary<int, Event>();
      this.Events = new Dictionary<int, Event>();
      this.Commission = 8;
      this.Enlistment = 7;
      this.Description = description;
      this.Category = category;
      this.CareerFinishedOnFailedAssignmentChange = false;
      this.AllowUseOfAnagathics = true;
    }

    public Career(
      string name,
      string description,
      int previous_careers_dm,
      int max_age,
      int max_age_dm,
      int advancedEduLevelRequired,
      string category,
      string enlistementStat,
      int enlistment,
      bool isDraftCareer,
      string commisionStat,
      int commision,
      bool useSpecialistSkillListForBasicTraining,
      bool hasAdvancedEducation,
      Career.Specialization[] specializations,
      Outcome[] serviceSkills,
      Outcome[] personalDevelopment,
      Outcome[] officeSkills,
      Outcome[] advancedEducation,
      Outcome[] psionicSkills,
      Dictionary<int, Event> events,
      Dictionary<int, Event> mishaps,
      Dictionary<int, Rank> commisionedRanks,
      Dictionary<int, Rank> careerRanks,
      Dictionary<int, MusteringOutBenefit> musteringOutBenefits)
      : this(name, description, previous_careers_dm, max_age, max_age_dm, advancedEduLevelRequired, category, enlistementStat, enlistment, isDraftCareer, commisionStat, commision, useSpecialistSkillListForBasicTraining, hasAdvancedEducation, specializations, serviceSkills, personalDevelopment, officeSkills, advancedEducation, events, mishaps, commisionedRanks, careerRanks, musteringOutBenefits)
    {
      this.psionicSkills = psionicSkills;
    }

    public Career(
      string name,
      string description,
      int previous_careers_dm,
      int max_age,
      int max_age_dm,
      int advancedEduLevelRequired,
      string category,
      string enlistementStat,
      int enlistment,
      bool isDraftCareer,
      string commisionStat,
      int commision,
      bool useSpecialistSkillListForBasicTraining,
      bool hasAdvancedEducation,
      Career.Specialization[] specializations,
      Outcome[] serviceSkills,
      Outcome[] personalDevelopment,
      Outcome[] officeSkills,
      Outcome[] advancedEducation,
      Dictionary<int, Event> events,
      Dictionary<int, Event> mishaps,
      Dictionary<int, Rank> commisionedRanks,
      Dictionary<int, Rank> careerRanks,
      Dictionary<int, MusteringOutBenefit> musteringOutBenefits)
      : this(name, description, category)
    {
      if (max_age_dm != 0)
      {
        Console.WriteLine("SETTING MAX AGE FOR " + name + ": " + max_age.ToString() + ", Modifier: " + max_age_dm.ToString());
        this.qualModifiers.Add((CareerQualModifier) new OldAgeQualModifier(max_age, max_age_dm));
      }
      if (previous_careers_dm > -1000)
        this.qualModifiers.Add((CareerQualModifier) new PreviousCareerQualModifier(previous_careers_dm));
      this.AdvancedEducationLevelRequired = advancedEduLevelRequired;
      if (enlistementStat != null && enlistementStat.Any<char>())
        this.qualChallenges.Add((CareerChallenge) new QualStatChallenge(enlistementStat, enlistment));
      this.DraftCareer = isDraftCareer;
      this.useSpecialistSkillListForBasicTraining_Renamed = useSpecialistSkillListForBasicTraining;
      this.hasAdvancedEducation = hasAdvancedEducation;
      if (specializations != null && ((IEnumerable<Career.Specialization>) specializations).Any<Career.Specialization>())
        this.specializations.AddRange((IEnumerable<Career.Specialization>) specializations);
      this.serviceSkills = serviceSkills;
      this.personalDevelopment = personalDevelopment;
      this.officerSkills = officeSkills;
      this.advancedEducation = advancedEducation;
      this.Events = events;
      this.Mishaps = mishaps;
      this.commisionedRanks = commisionedRanks;
      this.careerRanks = careerRanks;
      this.musteringOutBenefits = musteringOutBenefits;
      this.CommissionStat = commisionStat;
      this.Commission = commision;
      if (careerRanks != null)
        return;
      this.careerRanks = new Dictionary<int, Rank>();
    }

    public void addRequiredTrait(Trait addMe)
    {
      this.requiredTraits.Add(addMe);
      this.Category = this.Category != null && this.Category.Length != 0 ? this.Category + ", " + this.Name : addMe.Name;
      this.qualFilters.Add((CareerQualFilter) new TraitQualFilter(addMe));
    }

    public void removeRequiredTrait(Trait removeMe)
    {
      this.requiredTraits.Remove(removeMe);
      for (int index = 0; index < this.qualFilters.Count; ++index)
      {
        if (this.qualFilters[index] is TraitQualFilter && ((TraitQualFilter) this.qualFilters[index]).mustHave.Name.Equals(removeMe.Name, StringComparison.CurrentCultureIgnoreCase))
          this.qualFilters.RemoveAt(index--);
      }
    }

    public List<Trait> getRequiredTraits()
    {
      return new List<Trait>((IEnumerable<Trait>) this.requiredTraits);
    }

    public int qualificationChancePercent(Character character, int modifier = 0)
    {
      int num1 = 0;
      if (this.passesQualFilters(character))
      {
        int totalModifier = modifier + this.calculateQualModifier(character);
        Console.WriteLine("QualPercentChanceModifier: " + modifier.ToString());
        if (this.qualChallenges.Count > 0)
        {
          foreach (CareerChallenge qualChallenge in this.qualChallenges)
          {
            int num2 = qualChallenge.percentChanceOfSuccess(character, totalModifier);
            if (num2 > num1)
            {
              num1 = num2;
              Console.WriteLine("Found a qual prob of: " + num2.ToString());
            }
          }
        }
        else
          num1 = 100;
      }
      else
        num1 = 0;
      Console.WriteLine("Returning ProbabilityPC: " + num1.ToString());
      return num1;
    }

    public List<RollParam> qualificationRolls(Character forCharacter, int withModifier = 0)
    {
      withModifier += this.calculateQualModifier(forCharacter);
      if (!this.passesQualFilters(forCharacter))
      {
        EngineLog.Print("Character didn't pass filters...");
        return new List<RollParam>()
        {
          new RollParam(new Attribute("Not Allowed", 6, 2, 0)
          {
            UninjuredValue = 0,
            Value = 0
          }, 15, "Didn't pass filters")
        };
      }
      List<RollParam> source = new List<RollParam>();
      foreach (CareerChallenge qualChallenge in this.qualChallenges)
      {
        RollParam rollParam = qualChallenge.qualificationRoll(forCharacter, withModifier);
        if (rollParam == null)
          EngineLog.Error("null roll in challenge for career qualification! Career.qualificationRolls()");
        else
          source.Add(rollParam);
      }
      return source.OrderByDescending<RollParam, int>((Func<RollParam, int>) (rp => rp.totalModifier + rp.rawMinSuccessValue)).ToList<RollParam>();
    }

    private bool passesQualFilters(Character character)
    {
      bool flag = true;
      if (this.qualFilters.Count > 0)
      {
        foreach (CareerQualFilter qualFilter in this.qualFilters)
        {
          flag = qualFilter.passFilter(character);
          if (!flag)
            break;
        }
      }
      return flag;
    }

    private int calculateQualModifier(Character character)
    {
      int qualModifier1 = 0;
      if (this.qualModifiers.Count > 0)
      {
        foreach (CareerQualModifier qualModifier2 in this.qualModifiers)
        {
          qualModifier1 += qualModifier2.getQualModifier(character);
          if (qualModifier2.getQualModifier(character) == int.MinValue)
            Console.WriteLine("Bogus Qual Modifier (MinValue).... detected");
        }
      }
      return qualModifier1;
    }

    public int commmisionChancePercent(Character character, int modifier = 0)
    {
      if (!this.hasCommissions())
        return 0;
      int num = Dice.ProbabilityPercent(this.commisionRoll(character));
      EngineLog.Print("commisionChancePercent:  Atr: " + this.CommissionStat + " target: " + this.Commission.ToString());
      return num;
    }

    [JsonProperty]
    public virtual bool DraftCareer { get; private set; }

    [JsonProperty]
    public virtual int Enlistment { get; private set; }

    [JsonProperty]
    public virtual int Commission { get; private set; }

    [JsonProperty]
    public virtual string CommissionStat { get; private set; }

    public virtual string EnlistmentStat
    {
      get
      {
        string str = (string) null;
        if (this.qualChallenges != null && this.qualChallenges.Count > 0)
        {
          foreach (CareerChallenge qualChallenge in this.qualChallenges)
          {
            if (qualChallenge is QualStatChallenge)
            {
              QualStatChallenge qualStatChallenge = (QualStatChallenge) qualChallenge;
              str = str != null ? str + " or " + qualStatChallenge.stat_name : qualStatChallenge.stat_name;
            }
          }
        }
        return str ?? "None";
      }
    }

    public virtual bool hasCommissions()
    {
      return this.commisionedRanks != null && this.commisionedRanks.Count > 0;
    }

    public virtual bool useSpecialistSkillListForBasicTraining()
    {
      return this.useSpecialistSkillListForBasicTraining_Renamed;
    }

    [JsonProperty]
    public virtual string Description { get; set; }

    [JsonProperty]
    public virtual Dictionary<int, Event> Events { get; private set; }

    [JsonProperty]
    public virtual Dictionary<int, Event> Mishaps { get; private set; }

    public virtual string QualStat => this.EnlistmentStat;

    public virtual int QualValue => this.Enlistment;

    public virtual int PreviousCareerDM { get; private set; }

    public virtual IList<MusteringOutBenefit> MustringOutBenefitList
    {
      get
      {
        List<MusteringOutBenefit> mustringOutBenefitList = new List<MusteringOutBenefit>();
        if (this.musteringOutBenefits != null)
        {
          mustringOutBenefitList.AddRange((IEnumerable<MusteringOutBenefit>) this.musteringOutBenefits.Values);
          mustringOutBenefitList.Sort((IComparer<MusteringOutBenefit>) new Career.ComparatorAnonymousInnerClassHelper(this));
        }
        return (IList<MusteringOutBenefit>) mustringOutBenefitList;
      }
    }

    public virtual IList<Rank> CareerRanks
    {
      get
      {
        List<Rank> careerRanks = new List<Rank>();
        if (this.careerRanks != null && this.careerRanks.Count > 0)
        {
          careerRanks.AddRange((IEnumerable<Rank>) this.careerRanks.Values);
          careerRanks.Sort((IComparer<Rank>) new Career.ComparatorAnonymousInnerClassHelper2(this));
        }
        return (IList<Rank>) careerRanks;
      }
    }

    [JsonProperty]
    public virtual IList<Rank> OfficerRanks
    {
      get
      {
        List<Rank> officerRanks = new List<Rank>();
        if (this.commisionedRanks != null && this.commisionedRanks.Count > 0)
        {
          officerRanks.AddRange((IEnumerable<Rank>) this.commisionedRanks.Values);
          officerRanks.Sort((IComparer<Rank>) new Career.ComparatorAnonymousInnerClassHelper3(this));
        }
        return (IList<Rank>) officerRanks;
      }
    }

    [JsonProperty]
    public virtual int AdvancedEducationLevelRequired { get; private set; }

    [JsonProperty]
    public int SocLevelRequiredForPsionics { get; set; }

    [JsonProperty]
    public virtual IList<Outcome> PersonalDevelopment
    {
      get
      {
        return (IList<Outcome>) this.deepCopySkillTable((IEnumerable<Outcome>) this.personalDevelopment);
      }
    }

    private List<Outcome> deepCopySkillTable(IEnumerable<Outcome> copyMe)
    {
      return copyMe == null ? new List<Outcome>() : copyMe.Select<Outcome, Outcome>((Func<Outcome, Outcome>) (x => x.Clone<Outcome>())).ToList<Outcome>();
    }

    [JsonIgnore]
    public virtual IList<Outcome> ServiceSkills
    {
      get => (IList<Outcome>) this.deepCopySkillTable((IEnumerable<Outcome>) this.serviceSkills);
    }

    [JsonIgnore]
    public virtual List<Outcome> AdvancedEducationList
    {
      get => this.deepCopySkillTable((IEnumerable<Outcome>) this.advancedEducation);
      set => this.advancedEducation = value.ToArray();
    }

    [JsonIgnore]
    public virtual IList<Outcome> OfficerSkills
    {
      get
      {
        return this.officerSkills == null ? (IList<Outcome>) new List<Outcome>() : (IList<Outcome>) this.deepCopySkillTable((IEnumerable<Outcome>) this.officerSkills);
      }
    }

    public IList<Outcome> PsionicSkills
    {
      get
      {
        return this.psionicSkills == null ? (IList<Outcome>) new List<Outcome>() : (IList<Outcome>) this.deepCopySkillTable((IEnumerable<Outcome>) this.psionicSkills);
      }
      set => this.psionicSkills = this.deepCopySkillTable((IEnumerable<Outcome>) value).ToArray();
    }

    public virtual IList<Outcome> getSpecializationSkills(string specialization)
    {
      IList<Outcome> specializationSkills = (IList<Outcome>) new List<Outcome>();
      Career.Specialization specialization1 = this.getSpecialization(specialization);
      if (specialization1 != null && specialization1.specializationSkills != null)
        specializationSkills = (IList<Outcome>) this.deepCopySkillTable((IEnumerable<Outcome>) specialization1.specializationSkills);
      return specializationSkills;
    }

    public virtual bool qualifyForCareer(Character character, int qualificationModifier)
    {
      EngineLog.Print("rolling for career");
      bool flag = false;
      if (this.passesQualFilters(character))
      {
        int additionalModifier = qualificationModifier + this.calculateQualModifier(character);
        this.qualChallenges = this.qualChallenges.OrderByDescending<CareerChallenge, int>((Func<CareerChallenge, int>) (qc => qc.percentChanceOfSuccess(character, 0))).ToList<CareerChallenge>();
        flag = this.qualChallenges.First<CareerChallenge>().qualify(character, additionalModifier).isSuccessful;
      }
      return flag;
    }

    public RollParam commisionRoll(Character character, int customModifier = 0)
    {
      Console.Write("Entering commisionRoll: Commision State (" + this.CommissionStat + ")");
      Attribute attribute = character.getAttribute(this.CommissionStat);
      if (attribute == null)
      {
        Console.Write("[TARGET ATTRIBUTE IS NULL!]");
        EngineLog.Error("[TARGET ATTRIBUTE IS NULL!]");
      }
      Console.Write("[Obtaining Current Term Advancement Modifier] ");
      int advancementModifier = character.CurrentTerm != null ? character.CurrentTerm.advancementModifier : 0;
      Console.Write("[Creating RollParam] ");
      RollParam rollParam = new RollParam();
      Console.Write("[Setting Attribute] ");
      rollParam.attribute = attribute;
      int num = this.Commission;
      Console.Write("[Setting Target (" + num.ToString() + ")] ");
      rollParam.rawMinSuccessValue = this.Commission;
      if (attribute != null)
      {
        string[] strArray = new string[5]
        {
          "[Setting TotalModifier (",
          customModifier.ToString(),
          "+",
          null,
          null
        };
        num = attribute.Modifier;
        strArray[3] = num.ToString();
        strArray[4] = "] ";
        Console.Write(string.Concat(strArray));
        rollParam.totalModifier = customModifier + attribute.Modifier + advancementModifier;
      }
      else
        rollParam.totalModifier = customModifier + advancementModifier;
      Console.WriteLine(" [returning rp]");
      return rollParam;
    }

    protected internal virtual IList<Outcome> tryForAdvancement(
      Character character,
      string specialization,
      bool tryForCommision,
      int additional_modifier = 0)
    {
      List<Outcome> outcomeList = new List<Outcome>();
      Term currentTerm = character.CurrentTerm;
      RollParam setup;
      Rank rank;
      string successMessage;
      string failureMessage;
      if (tryForCommision)
      {
        int num = (character.CurrentTerm.numberOfTermsInCareerIncludingThisOne - 1) * -1;
        setup = this.commisionRoll(character, additional_modifier + num);
        rank = this.getRank(true, currentTerm.rank + 1, currentTerm.specializationName);
        successMessage = "\r\nCongratulations! You were commissioned.";
        failureMessage = "Sorry, in the eyes of your superiors you aren't officer material.";
      }
      else
      {
        setup = this.getSpecialization(specialization).advancementRoll(character);
        successMessage = "\r\nCongratulations! You were advanced.";
        rank = this.getRank(currentTerm.officer, currentTerm.rank + 1, currentTerm.specializationName);
        failureMessage = "Sorry, you were passed over for promotion.";
      }
      if (rank != null)
      {
        successMessage = successMessage + " " + currentTerm.careerName + " " + rank.level.ToString();
        if (rank.title != null && rank.title.Length > 0)
          successMessage = successMessage + ": Your rank is now " + rank.title + ".";
      }
      this.currentAdvancementRollEffect = Dice.Roll(setup, successMessage, failureMessage);
      bool flag1 = this.currentAdvancementRollEffect.rawResult == 12;
      bool flag2 = this.currentAdvancementRollEffect.isSuccessful | flag1;
      bool flag3 = !flag1 && this.currentAdvancementRollEffect.rawResult < character.CurrentTerm.numberOfTermsInCareerIncludingThisOne;
      character.CurrentTerm.advancementModifier = 0;
      if (flag2)
      {
        if (tryForCommision)
          outcomeList.Add((Outcome) new Outcome.GainCommision());
        else
          outcomeList.Add((Outcome) new Event.GainPromotion());
      }
      else
        outcomeList.Add((Outcome) new Event("Passed promotion", "Sorry, you were passed over for promotion this term. It's time they start respecting you for all your hard work!"));
      if (flag1)
        outcomeList.Add((Outcome) new Event.MustContinueCareer("Continue career", "You're doing a great job! Not only did you get a promotion, they insist you stay another term."));
      else if (flag3)
        outcomeList.Add((Outcome) new Event.EndOfCareer("After " + character.CurrentTerm.numberOfTermsInCareerIncludingThisOne.ToString() + " terms, this will be your last.  It's just time to leave."));
      return (IList<Outcome>) outcomeList;
    }

    public virtual IList<Outcome> qualifyForAdvancement(
      Character character,
      string specialization,
      int modifier = 0)
    {
      return this.tryForAdvancement(character, specialization, false, modifier);
    }

    public virtual IList<Outcome> qualifyForCommision(Character character, int modifier = 0)
    {
      return this.tryForAdvancement(character, "", true, modifier);
    }

    public virtual Career.Specialization getSpecialization(string specialization)
    {
      return this.specializations.FirstOrDefault<Career.Specialization>((Func<Career.Specialization, bool>) (t => t.Name.Equals(specialization, StringComparison.CurrentCultureIgnoreCase)));
    }

    [JsonIgnore]
    public virtual Event Event
    {
      get
      {
        Event instance = new Event();
        if (this.Events != null)
          instance = Dice.RollRandomResult<Event>(this.Name + " Term Events: Things Happen...", (IList<Event>) new List<Event>((IEnumerable<Event>) this.Events.Values), ContextKeys.OUTCOMES);
        return instance.Clone<Event>();
      }
    }

    [JsonIgnore]
    public virtual Event Mishap
    {
      get
      {
        Event instance = new Event();
        if (this.Events != null)
          instance = Dice.RollRandomResult<Event>("Bad things happen...", (IList<Event>) new List<Event>((IEnumerable<Event>) this.Mishaps.Values), ContextKeys.OUTCOMES);
        return instance.Clone<Event>();
      }
    }

    private int getMinimumEventNumber(Dictionary<int, Event> eventsToExamine)
    {
      List<int> intList = new List<int>((IEnumerable<int>) eventsToExamine.Keys);
      intList.Sort();
      return intList[0];
    }

    public virtual IList<NamedList<Outcome>> getAvailableTables(Character character)
    {
      if (this.ServiceSkills == null || this.ServiceSkills.Count == 0 || this.PersonalDevelopment == null || this.PersonalDevelopment.Count == 0)
        EngineLog.Error("Career [" + this.Name + "] don't have one of basic skill tables");
      List<NamedList<Outcome>> availableTables = new List<NamedList<Outcome>>()
      {
        new NamedList<Outcome>("Personal Development", (ICollection<Outcome>) this.PersonalDevelopment),
        new NamedList<Outcome>("Service Skills", (ICollection<Outcome>) this.ServiceSkills)
      };
      string specializationName = character.CurrentTerm.specializationName;
      NamedList<Outcome> namedList = new NamedList<Outcome>(specializationName, (ICollection<Outcome>) this.getSpecializationSkills(specializationName));
      if (namedList != null && namedList.Count > 0)
        availableTables.Add(namedList);
      if (this.hasAdvancedEducation && character.getAttributeValue("Edu") >= this.AdvancedEducationLevelRequired)
        availableTables.Add(new NamedList<Outcome>("Advanced Education", (ICollection<Outcome>) this.AdvancedEducationList));
      if (character.CurrentTerm.officer && this.hasCommissions())
        availableTables.Add(new NamedList<Outcome>("Officer Skills", (ICollection<Outcome>) this.OfficerSkills));
      Attribute attribute = character.getAttribute("psi");
      if (this.psionicSkills != null && this.psionicSkills.Length != 0 && attribute != null && !((IEnumerable<Outcome>) this.psionicSkills).Any<Outcome>((Func<Outcome, bool>) (s => s == null)) && character.getAttribute("soc").Value >= this.SocLevelRequiredForPsionics)
        availableTables.Add(new NamedList<Outcome>("Psionics", (ICollection<Outcome>) this.PsionicSkills));
      if (availableTables.Count == 0)
        EngineLog.Error("Career.geAvailableTables() has no tables for career: " + this.Name);
      return (IList<NamedList<Outcome>>) availableTables;
    }

    public virtual Rank getRank(bool isCommissioned, int rank, string specialization)
    {
      Rank rank1 = (Rank) null;
      if (isCommissioned && this.hasCommissions())
      {
        EngineLog.Print("inside get rank, has commissions and is commissioned");
        Dictionary<int, Rank> dictionary = new Dictionary<int, Rank>();
        foreach (Rank rank2 in this.commisionedRanks.Values)
          dictionary[rank2.level] = rank2;
        dictionary.TryGetValue(rank, out rank1);
        if (rank1 == null)
          EngineLog.Warning("Not enough CommisionedRanks for Rank " + rank.ToString() + " in Career: " + this.Name);
      }
      else
      {
        EngineLog.Print("inside get rank, no commissions or isn't commissioned");
        Career.Specialization specialization1 = this.getSpecialization(specialization);
        if (specialization1 == null)
          Console.WriteLine("Error.  Specialization shouldn't be null when getting ranks");
        EngineLog.Print(string.Format("inside get rank, specialization null state {0}", (object) (specialization1 == null)));
        rank1 = specialization1.getRank(rank);
        if (rank1 == null && this.careerRanks != null)
          this.careerRanks.TryGetValue(rank, out rank1);
      }
      EngineLog.Print(string.Format("inside get rank, about to return  retval null state{0} ", (object) (rank1 == null)));
      return rank1 ?? new Rank(rank, "");
    }

    public virtual IList<Career.Specialization> GetApplicableSpecializations(Character character)
    {
      return (IList<Career.Specialization>) this.specializations.Where<Career.Specialization>((Func<Career.Specialization, bool>) (s => s.MayEnterThisSpecialization(character))).ToList<Career.Specialization>();
    }

    [JsonIgnore]
    public virtual IList<Career.Specialization> Specializations
    {
      get
      {
        return (IList<Career.Specialization>) new List<Career.Specialization>((IEnumerable<Career.Specialization>) this.specializations);
      }
    }

    public virtual int getCashBenefit(int roll)
    {
      if (roll > this.musteringOutBenefits.Count)
        roll = this.musteringOutBenefits.Count;
      return this.musteringOutBenefits[roll].cash;
    }

    public virtual Outcome getBenefit(int roll)
    {
      if (roll >= this.musteringOutBenefits.Count)
        roll = this.musteringOutBenefits.Count - 1;
      return this.musteringOutBenefits[roll].benefit.Clone<Outcome>();
    }

    public virtual int getNumBenefits(Character character)
    {
      Term currentTerm = character.CurrentTerm;
      int rank = currentTerm.rank;
      int alreadyAwaredInCareer = character.getNumMusteringOutBenefitsAlreadyAwaredInCareer(this.Name);
      int numBenefits = currentTerm.numberOfTermsInCareerIncludingThisOne + currentTerm.additional_mustering_out_benefits - alreadyAwaredInCareer;
      if (rank >= 1)
      {
        if (rank <= 2)
          ++numBenefits;
        else if (rank <= 4)
          numBenefits += 2;
        else
          numBenefits += 3;
      }
      return numBenefits;
    }

    public override bool Equals(object amIEqual)
    {
      bool flag = false;
      if (amIEqual == this)
        flag = true;
      else if (amIEqual is Career && this.Name.Equals(((Career) amIEqual).Name, StringComparison.CurrentCultureIgnoreCase))
        flag = true;
      return flag;
    }

    public override int GetHashCode() => this.Name != null ? this.Name.GetHashCode() : 0;

    public override string ToString() => this.Name;

    public Guid Id { get; set; }

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    internal void AwardBenefit(Character character)
    {
      ++character.CurrentTerm.num_mustering_out_benefits_awarded_this_term;
    }

    private class ComparatorAnonymousInnerClassHelper : IComparer<MusteringOutBenefit>
    {
      private readonly Career outerInstance;

      public ComparatorAnonymousInnerClassHelper(Career outerInstance)
      {
        this.outerInstance = outerInstance;
      }

      public virtual int Compare(MusteringOutBenefit o1, MusteringOutBenefit o2)
      {
        return o1.roll - o2.roll;
      }
    }

    private class ComparatorAnonymousInnerClassHelper2 : IComparer<Rank>
    {
      private readonly Career outerInstance;

      public ComparatorAnonymousInnerClassHelper2(Career outerInstance)
      {
        this.outerInstance = outerInstance;
      }

      public virtual int Compare(Rank o1, Rank o2) => o1.level - o2.level;
    }

    private class ComparatorAnonymousInnerClassHelper3 : IComparer<Rank>
    {
      private readonly Career outerInstance;

      public ComparatorAnonymousInnerClassHelper3(Career outerInstance)
      {
        this.outerInstance = outerInstance;
      }

      public virtual int Compare(Rank o1, Rank o2) => o1.level - o2.level;
    }

    public sealed class Specialization : IDescribable
    {
      [JsonProperty]
      public Outcome[] specializationSkills = new Outcome[6];
      [JsonProperty]
      private Dictionary<int, Rank> ranks = new Dictionary<int, Rank>();
      [JsonProperty]
      private List<CareerQualFilter> qualificationFilters = new List<CareerQualFilter>();

      [JsonConstructor]
      public Specialization()
      {
      }

      public Specialization(
        string name,
        string description,
        string survivalStat,
        int survival,
        string advancementStat,
        int advancement,
        Outcome[] specializationSkills,
        Dictionary<int, Rank> ranks)
      {
        this.Name = name;
        this.Description = description;
        this.SurvivalStat = survivalStat;
        this.Survival = survival;
        this.AdvancementStat = advancementStat;
        this.Advancement = advancement;
        this.specializationSkills = specializationSkills;
        this.ranks = ranks;
      }

      public void AddQualFilter(CareerQualFilter addMe) => this.qualificationFilters.Add(addMe);

      public Rank getRank(int rank)
      {
        Rank rank1 = (Rank) null;
        if (this.ranks != null)
          this.ranks.TryGetValue(rank, out rank1);
        return rank1;
      }

      public string Name { get; set; }

      public string Description { get; set; }

      public string SurvivalStat { get; set; }

      public int Survival { get; set; }

      public string AdvancementStat { get; set; }

      public int Advancement { get; set; }

      public int advancementChancePercent(Character character, int modifier = 0)
      {
        return Dice.ProbabilityPercent(this.advancementRoll(character, modifier));
      }

      public RollParam advancementRoll(Character forCharacter, int modifier = 0)
      {
        Console.Write("[Starting advancementRoll- creating RP]");
        RollParam rollParam = new RollParam();
        Console.Write("[setting attribute RP (" + this.AdvancementStat + ")]");
        rollParam.attribute = forCharacter.getAttribute(this.AdvancementStat);
        Console.Write("[setting target to " + this.Advancement.ToString() + "]");
        rollParam.rawMinSuccessValue = this.Advancement;
        Console.Write("[Setting advancementMod]");
        int advancementModifier = forCharacter.CurrentTerm != null ? forCharacter.CurrentTerm.advancementModifier : 0;
        Console.Write("[Setting RP.totalModifier]");
        rollParam.totalModifier = rollParam.attribute.Modifier + advancementModifier + modifier;
        if (forCharacter.CurrentTerm != null)
          rollParam.description = "Term [" + forCharacter.CurrentTerm.term.ToString() + "] " + forCharacter.CurrentTerm.careerName + ": " + this.Name + " Advancement Roll";
        else
          rollParam.description = "";
        Console.WriteLine("[Returning RP]");
        return rollParam;
      }

      public RollParam survivalRoll(Character forCharacter, int modifier = 0)
      {
        RollParam rollParam = new RollParam();
        rollParam.attribute = forCharacter.getAttribute(this.SurvivalStat);
        rollParam.rawMinSuccessValue = this.Survival;
        int survivalModifier = forCharacter.CurrentTerm != null ? forCharacter.CurrentTerm.survivalModifier : 0;
        rollParam.totalModifier = rollParam.attribute.Modifier + survivalModifier + modifier;
        if (forCharacter.CurrentTerm != null)
          rollParam.description = "Term [" + forCharacter.CurrentTerm.term.ToString() + "] " + forCharacter.CurrentTerm.careerName + ": " + this.Name + " Survival Roll";
        else
          rollParam.description = "";
        return rollParam;
      }

      public IList<string> SpecializationBenefitList
      {
        get
        {
          List<string> specializationBenefitList = new List<string>();
          if (this.specializationSkills != null)
            specializationBenefitList.AddRange(((IEnumerable<Outcome>) this.specializationSkills).Where<Outcome>((Func<Outcome, bool>) (outcome => outcome != null)).Select<Outcome, string>((Func<Outcome, string>) (outcome => outcome.ToString())));
          return (IList<string>) specializationBenefitList;
        }
      }

      public Outcome getSpecializationBenefit(int roll)
      {
        return roll >= 1 && roll <= 6 ? this.specializationSkills[roll - 1].Clone<Outcome>() : throw new ArgumentException("die rolls must be between 1 and 6");
      }

      public override string ToString() => this.Name;

      [JsonProperty]
      public Outcome[] SpecializationOutcomeList => this.specializationSkills;

      public IList<Rank> Ranks
      {
        get
        {
          List<Rank> ranks = new List<Rank>();
          if (this.ranks != null && this.ranks.Count > 0)
            ranks.AddRange((IEnumerable<Rank>) this.ranks.Values);
          return (IList<Rank>) ranks;
        }
      }

      internal bool MayEnterThisSpecialization(Character character)
      {
        bool flag = true;
        if (this.qualificationFilters == null || !this.qualificationFilters.Any<CareerQualFilter>())
        {
          flag = true;
        }
        else
        {
          foreach (CareerQualFilter qualificationFilter in this.qualificationFilters)
          {
            flag = qualificationFilter.passFilter(character);
            if (!flag)
              break;
          }
        }
        return flag;
      }
    }
  }
}
