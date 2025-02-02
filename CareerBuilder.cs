
// Type: com.digitalarcsystems.Traveller.CareerBuilder




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.utility;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class CareerBuilder
  {
    private List<SpecializationBuilder> specializationBuilders = new List<SpecializationBuilder>();
    private static readonly int DIDNT_CONVERT = -2147483647;
    private string name_Renamed = (string) null;
    private string description_Renamed;
    private int previous_career_dm = int.MinValue;
    private int old_age_dm = 0;
    private bool has_AE = false;
    private int education_value = int.MinValue;
    private List<Outcome> advancedEducationTable_Renamed = new List<Outcome>();
    private string categories = (string) null;
    private string qualStat = (string) null;
    private int qual_target = int.MinValue;
    private List<CareerQualModifier> qualMods = new List<CareerQualModifier>();
    private List<CareerQualFilter> qualFilters = new List<CareerQualFilter>();
    private List<CareerChallenge> qualChallenges = new List<CareerChallenge>();
    private string commStat = (string) null;
    private int commTarget = int.MinValue;
    private bool isDraftCareer = false;
    private int oldAge_Renamed = int.MaxValue;
    private bool spec_skills_for_basic_training = false;
    private List<Outcome> serviceTable_Renamed = new List<Outcome>();
    private List<Outcome> personalDevelopment = new List<Outcome>();
    private List<Outcome> officerTable_Renamed = new List<Outcome>();
    private List<Outcome> psionicsTable_Ranamed = new List<Outcome>();
    private Dictionary<int, Event> events_Renamed = (Dictionary<int, Event>) null;
    private Dictionary<int, Event> mishaps_Renamed = (Dictionary<int, Event>) null;
    private Dictionary<int, Rank> officerRanks_Renamed = (Dictionary<int, Rank>) null;
    private Dictionary<int, Rank> careerRanks_Renamed = (Dictionary<int, Rank>) null;
    private Dictionary<int, MusteringOutBenefit> mobBenefits = (Dictionary<int, MusteringOutBenefit>) null;
    private List<Trait> requiredTraits = new List<Trait>();
    private int _socLevelForPsionics = 10;
    private bool automaticCommissionIfStatIsAtOrOverTarget = false;

    private CareerBuilder.TristateBoolean CareerFinishedOnFailedAssignmentChange { get; set; }

    public CareerBuilder()
    {
      this.CareerFinishedOnFailedAssignmentChange = CareerBuilder.TristateBoolean.NOT_SET;
    }

    public virtual CareerBuilder setCareerFinishedOnFailedAssignmentChange(
      bool finishCareerOnFailedAssignmentChange)
    {
      this.CareerFinishedOnFailedAssignmentChange = finishCareerOnFailedAssignmentChange ? CareerBuilder.TristateBoolean.TRUE : CareerBuilder.TristateBoolean.FALSE;
      return this;
    }

    public virtual CareerBuilder add(SpecializationBuilder specialization)
    {
      this.specializationBuilders.Add(specialization);
      return this;
    }

    public virtual CareerBuilder addSpecialization(SpecializationBuilder addMe)
    {
      this.add(addMe);
      return this;
    }

    public virtual CareerBuilder name(string careerName)
    {
      this.name_Renamed = careerName;
      return this;
    }

    public virtual CareerBuilder description(string careerDescription)
    {
      this.description_Renamed = careerDescription;
      return this;
    }

    public virtual CareerBuilder PreviousCareerDM(string previousCareer)
    {
      int result;
      this.previous_career_dm = int.TryParse(previousCareer, out result) ? result : CareerBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual CareerBuilder requiredTrait(Trait required)
    {
      this.requiredTraits.Add(required);
      return this;
    }

    public virtual CareerBuilder OldAge(string oldAge)
    {
      int result;
      this.oldAge_Renamed = int.TryParse(oldAge, out result) ? result : CareerBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual CareerBuilder OldAgeDM(string oldAgeDM)
    {
      int result;
      this.old_age_dm = int.TryParse(oldAgeDM, out result) ? result : CareerBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual CareerBuilder hasAdvancedEducation(bool has_advanced_education)
    {
      this.has_AE = has_advanced_education;
      return this;
    }

    public virtual CareerBuilder advancedEducation(string educationValueRequired)
    {
      int result;
      this.education_value = int.TryParse(educationValueRequired, out result) ? result : CareerBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual CareerBuilder draftCareer(bool isDraftCareer)
    {
      this.isDraftCareer = isDraftCareer;
      return this;
    }

    public virtual CareerBuilder advancedEducationTable(Outcome[] educationOutcomes)
    {
      if (educationOutcomes != null && educationOutcomes.Length != 0)
        this.has_AE = true;
      this.advancedEducationTable_Renamed.Clear();
      this.advancedEducationTable_Renamed.AddRange((IEnumerable<Outcome>) educationOutcomes);
      return this;
    }

    public virtual CareerBuilder careerCategories(string careerCategories)
    {
      this.categories = careerCategories;
      return this;
    }

    public virtual CareerBuilder qualificationStat(string qualificationStat)
    {
      this.qualStat = qualificationStat;
      return this;
    }

    public virtual CareerBuilder addQualModifier(CareerQualModifier addMe)
    {
      this.qualMods.Add(addMe);
      return this;
    }

    public virtual CareerBuilder addQualFilter(CareerQualFilter addMe)
    {
      this.qualFilters.Add(addMe);
      return this;
    }

    public virtual CareerBuilder addQualChallenge(CareerChallenge addMe)
    {
      this.qualChallenges.Add(addMe);
      return this;
    }

    public virtual CareerBuilder qualificationTarget(string qualification_target)
    {
      int result;
      this.qual_target = int.TryParse(qualification_target, out result) ? result : CareerBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual CareerBuilder commissionStat(string commissionStat)
    {
      this.commStat = commissionStat;
      return this;
    }

    public virtual CareerBuilder commissionTarget(string commissionTarget)
    {
      int result;
      this.commTarget = int.TryParse(commissionTarget, out result) ? result : CareerBuilder.DIDNT_CONVERT;
      return this;
    }

    public CareerBuilder commisionIsAutomatic(bool promoteOnEntranceIfStatAtOrOverTarget)
    {
      this.automaticCommissionIfStatIsAtOrOverTarget = promoteOnEntranceIfStatAtOrOverTarget;
      return this;
    }

    public virtual CareerBuilder useSpecSkillsForBasicTraining(
      bool use_specialist_skills_for_basic_training)
    {
      this.spec_skills_for_basic_training = use_specialist_skills_for_basic_training;
      return this;
    }

    public virtual CareerBuilder serviceTable(Outcome[] serviceTableOutcomes)
    {
      this.serviceTable_Renamed.Clear();
      this.serviceTable_Renamed.AddRange((IEnumerable<Outcome>) serviceTableOutcomes);
      return this;
    }

    public virtual CareerBuilder personalDevelopmentTable(Outcome[] personalDevelopmentTableOutcomes)
    {
      this.personalDevelopment.Clear();
      this.personalDevelopment.AddRange((IEnumerable<Outcome>) personalDevelopmentTableOutcomes);
      return this;
    }

    public virtual CareerBuilder officerTable(Outcome[] officerTableOutcomes)
    {
      this.officerTable_Renamed.Clear();
      this.officerTable_Renamed.AddRange((IEnumerable<Outcome>) officerTableOutcomes);
      return this;
    }

    public CareerBuilder SocLevelRequiredForPsionics(int minSoc)
    {
      this._socLevelForPsionics = minSoc;
      return this;
    }

    public virtual CareerBuilder psionicsTable(Outcome[] psionicsTableOutcomes)
    {
      this.psionicsTable_Ranamed.Clear();
      this.psionicsTable_Ranamed.AddRange((IEnumerable<Outcome>) psionicsTableOutcomes);
      return this;
    }

    public virtual CareerBuilder events(Dictionary<int, Event> careerEvents)
    {
      this.events_Renamed = careerEvents;
      return this;
    }

    public virtual CareerBuilder addEvent(int roll, Event addMe)
    {
      if (this.events_Renamed == null)
        this.events_Renamed = new Dictionary<int, Event>();
      this.events_Renamed[roll] = addMe;
      return this;
    }

    public virtual CareerBuilder mishaps(Dictionary<int, Event> careerMishaps)
    {
      this.mishaps_Renamed = careerMishaps;
      return this;
    }

    public virtual CareerBuilder addMishap(int roll, Event mishap)
    {
      if (this.mishaps_Renamed == null)
        this.mishaps_Renamed = new Dictionary<int, Event>();
      this.mishaps_Renamed[roll] = mishap;
      return this;
    }

    public virtual CareerBuilder officerRanks(Dictionary<int, Rank> ranks)
    {
      this.officerRanks_Renamed = ranks;
      return this;
    }

    public virtual CareerBuilder addOfficerRanks(Rank addMe)
    {
      if (this.officerRanks_Renamed == null)
        this.officerRanks_Renamed = new Dictionary<int, Rank>();
      this.officerRanks_Renamed[addMe.level] = addMe;
      return this;
    }

    public virtual CareerBuilder careerRanks(Dictionary<int, Rank> careerRanks)
    {
      this.careerRanks_Renamed = careerRanks;
      return this;
    }

    public virtual CareerBuilder addCareerRank(Rank addMe)
    {
      if (this.careerRanks_Renamed == null)
        this.careerRanks_Renamed = new Dictionary<int, Rank>();
      this.careerRanks_Renamed[addMe.level] = addMe;
      return this;
    }

    public virtual CareerBuilder musteringOutBenefits(
      Dictionary<int, MusteringOutBenefit> musteringOutBenefitHash)
    {
      this.mobBenefits = musteringOutBenefitHash;
      CareerBuilder.EquipementMusteringOutBenefit oo = new CareerBuilder.EquipementMusteringOutBenefit();
      Utility.processAllContainedOutcomes((OutcomeOperator) oo, new Outcome());
      foreach (KeyValuePair<int, MusteringOutBenefit> mobBenefit in this.mobBenefits)
        Utility.processAllContainedOutcomes((OutcomeOperator) oo, mobBenefit.Value.benefit);
      return this;
    }

    public virtual CareerBuilder addMusteringOutBenefit(int roll, int cash, Outcome benefit)
    {
      if (this.mobBenefits == null)
        this.mobBenefits = new Dictionary<int, MusteringOutBenefit>();
      this.mobBenefits[roll] = new MusteringOutBenefit(roll, cash, benefit);
      return this;
    }

    public virtual IList<ValidationResult> validate()
    {
      List<ValidationResult> results = new List<ValidationResult>();
      IList<string> list = (IList<string>) ((IEnumerable<string>) com.digitalarcsystems.Traveller.DataModel.Attribute.CanonicalStats).ToList<string>();
      list.Add("Psi");
      if (this.name_Renamed == null || this.name_Renamed.Length == 0)
        results.Add(ValidationResult.error("Careers must have a valid name."));
      if (this.description_Renamed == null || this.description_Renamed.Length == 0)
        results.Add(ValidationResult.warning("The career has no description.  Its a good idea to give the career a solid description, but it isn't strictly necessary"));
      if (this.qualStat == null || this.qualStat.Length == 0)
        results.Add(ValidationResult.error("Careers must have a defined Qualification Stat"));
      if (!list.Contains(this.qualStat))
        results.Add(ValidationResult.warning("Qualification Stat is not a canonical stat"));
      this.validateInt(this.qual_target, "Qualification Target", results);
      this.validateInt(this.oldAge_Renamed, "Old Age", results, false);
      this.validateInt(this.old_age_dm, "Old Age DM", results, false);
      this.validateInt(this.previous_career_dm, "Previous Career DM", results, false);
      if (this.categories == null || this.categories.Length == 0)
        results.Add(ValidationResult.warning("It's generally a good idea to include a category or two in your career"));
      if (this.has_AE)
      {
        this.validate6OutcomesInTable((IList<Outcome>) this.advancedEducationTable_Renamed, "Advanced Education", (IList<ValidationResult>) results);
        this.validateInt(this.education_value, "Min Education Required for Advanced Education", results);
      }
      else if (this.advancedEducationTable_Renamed.Count > 0)
      {
        results.Add(ValidationResult.warning("You have selected to not have an Advanced Education for this career, but have configured Skills in the Advanced Education Table."));
        this.validateInt(this.education_value, "Min Education Required for Advanced Education", results);
      }
      this.validate6OutcomesInTable((IList<Outcome>) this.serviceTable_Renamed, "Service Skills Table", (IList<ValidationResult>) results);
      this.validate6OutcomesInTable((IList<Outcome>) this.personalDevelopment, "Personal Development Table", (IList<ValidationResult>) results);
      this.ensureNoRankDuplicates(this.careerRanks_Renamed, "Career", results);
      if (this.officerTable_Renamed.Count > 0)
      {
        this.validate6OutcomesInTable((IList<Outcome>) this.officerTable_Renamed, "Officer Skill Table", (IList<ValidationResult>) results);
        if (this.officerRanks_Renamed == null || this.officerRanks_Renamed.Count == 0)
          results.Add(ValidationResult.error("To have commissions in the class, you must have both officer ranks and outcomes in the officer table"));
        this.ensureNoRankDuplicates(this.officerRanks_Renamed, "Officer Ranks", results);
        if (this.commStat == null || this.commStat.Length == 0)
          results.Add(ValidationResult.error("Careers that have entries in the Officer Table must also define a commission stat."));
        else if (!list.Contains(this.commStat))
          results.Add(ValidationResult.warning("Commission Stat is not a canonical stat"));
        this.validateInt(this.commTarget, "Commision Target", results);
      }
      if (this.psionicsTable_Ranamed.Count > 0)
        this.validate6OutcomesInTable((IList<Outcome>) this.psionicsTable_Ranamed, "Psionic Skill Table", (IList<ValidationResult>) results);
      this.validateEvents(this.events_Renamed, "Career Events", results);
      if (this.events_Renamed != null && this.events_Renamed.Count > 0 && this.events_Renamed.Count != 11 && this.events_Renamed.Count != 36)
        results.Add(ValidationResult.warning(string.Format("Career Events contains [{0}] events.  Usual amounts are 11 and 36.", (object) this.events_Renamed.Count)));
      this.validateEvents(this.mishaps_Renamed, "Mishaps", results);
      if (this.mishaps_Renamed != null && this.mishaps_Renamed.Count > 0 && this.mishaps_Renamed.Count != 6)
        results.Add(ValidationResult.warning(string.Format("Mishaps have [{0}] the normal value is usually 6.", (object) this.mishaps_Renamed.Count)));
      if (this.specializationBuilders.Count != 3)
        results.Add(ValidationResult.error("There must be 3 specializations in a career."));
      else
        this.specializationBuilders.ForEach((Action<SpecializationBuilder>) (sb => results.AddRange((IEnumerable<ValidationResult>) sb.validate())));
      if (this.mobBenefits == null || this.mobBenefits.Count == 0)
      {
        results.Add(ValidationResult.error("Careers must have benefits.  No benefits found."));
      }
      else
      {
        foreach (KeyValuePair<int, MusteringOutBenefit> mobBenefit in this.mobBenefits)
        {
          if (mobBenefit.Value.benefit == null)
            results.Add(ValidationResult.error(string.Format("Mustering Out Benefit [{0}] must have a benefit.", (object) mobBenefit.Key)));
        }
        if (this.mobBenefits.Count < 7)
          results.Add(ValidationResult.warning(string.Format("Most careers have 7 mustering out benefits.  This one has [{0}]", (object) this.mobBenefits.Count)));
      }
      if (this.CareerFinishedOnFailedAssignmentChange == CareerBuilder.TristateBoolean.NOT_SET)
        results.Add(ValidationResult.error("CareerFinishedONFailedAssignmentChange NOT SET"));
      return (IList<ValidationResult>) results;
    }

    private void validateEvents(
      Dictionary<int, Event> events,
      string eventName,
      List<ValidationResult> results)
    {
      if (events == null || events.Count == 0)
      {
        results.Add(ValidationResult.error(string.Format("{0} must not be empty.", (object) eventName)));
      }
      else
      {
        IList<int> intList = (IList<int>) new List<int>();
        int num1 = -1;
        foreach (KeyValuePair<int, Event> keyValuePair in events)
        {
          int key = keyValuePair.Key;
          intList.Add(key);
          if (key < 0)
            results.Add(ValidationResult.error(string.Format("{0} numbers must be 0 or positive. [{1}] was found.", (object) eventName, (object) key)));
          if (num1 == -1)
          {
            num1 = key;
          }
          else
          {
            if (key != num1 + 1)
              results.Add(ValidationResult.error(string.Format("{0} are not number contiguously.  There is a break at [{1}].", (object) eventName, (object) key)));
            num1 = key;
          }
        }
        if (events.Count == 11)
        {
          foreach (int num2 in (IEnumerable<int>) intList)
          {
            if (num2 < 2 || num2 > 12)
              results.Add(ValidationResult.error(string.Format("{0} with 11 members must be numbered from 2-12. Found Event number [{1}]", (object) eventName, (object) num2)));
          }
        }
      }
    }

    private void ensureNoRankDuplicates(
      Dictionary<int, Rank> ranks,
      string rankTableName,
      List<ValidationResult> results)
    {
      if (ranks == null || ranks == null)
        return;
      IList<Rank> ranks1 = (IList<Rank>) new List<Rank>();
      foreach (Rank rank in ranks.Values)
        ranks1.Add(rank);
      this.ensureNoRankDuplicates(ranks1, rankTableName, results);
    }

    private void ensureNoRankDuplicates(
      IList<Rank> ranks,
      string rankTableName,
      List<ValidationResult> results)
    {
      if (ranks == null)
        return;
      if (ranks.Count > 8)
        results.Add(ValidationResult.warning(string.Format("There are a large number of ranks in the {0}. [{1}]", (object) rankTableName, (object) ranks.Count)));
      IList<int> intList = (IList<int>) new List<int>();
      foreach (Rank rank in (IEnumerable<Rank>) ranks)
      {
        if (intList.Contains(rank.level))
          results.Add(ValidationResult.error(string.Format("The {0} has duplicate {1} ranks", (object) rankTableName, (object) rank.level)));
        else
          intList.Add(rank.level);
      }
    }

    private void validate6OutcomesInTable(
      IList<Outcome> table,
      string tableName,
      IList<ValidationResult> results)
    {
      if (table.Count == 6)
        return;
      if (table.Count == 0)
        results.Add(ValidationResult.error(string.Format("{0} must be provided, but was missing.", (object) tableName)));
      else
        results.Add(ValidationResult.warning(string.Format("There were an unusual number of outcomes in the {0}. [{1}]  Normal is 6.", (object) tableName, (object) table.Count)));
    }

    private void validateInt(int target, string label, List<ValidationResult> results)
    {
      this.validateInt(target, label, results, true);
    }

    private void validateInt(
      int target,
      string label,
      List<ValidationResult> results,
      bool warn_on_unusual_value)
    {
      switch (target)
      {
        case int.MinValue:
          results.Add(ValidationResult.error(string.Format("Career must have a {0} value", (object) label)));
          break;
        case -2147483647:
          results.Add(ValidationResult.error(string.Format("Career {0} could not be converted from a string to an int", (object) label)));
          break;
        default:
          if (!warn_on_unusual_value)
            break;
          this.warnOnUnreasonableValue(target, label, (IList<ValidationResult>) results);
          break;
      }
    }

    private void warnOnUnreasonableValue(int target, string label, IList<ValidationResult> results)
    {
      if (target <= 12 && target >= 3)
        return;
      results.Add(ValidationResult.warning(string.Format("{0} is set to an unusually high or low number. [{1}]", (object) label, (object) target)));
    }

    public virtual Career createCareer()
    {
      Career.Specialization[] specializations = new Career.Specialization[this.specializationBuilders.Count];
      this.specializationBuilders.ForEach((Action<SpecializationBuilder>) (sb => specializations[this.specializationBuilders.IndexOf(sb)] = sb.createSpecialization()));
      Career career = new Career(this.name_Renamed, this.description_Renamed, this.previous_career_dm, this.oldAge_Renamed, this.old_age_dm, this.education_value < 0 ? 20 : this.education_value, this.categories == null ? "" : this.categories, this.qualStat, this.qual_target, this.isDraftCareer, this.commStat, this.commTarget < 0 ? 20 : this.commTarget, this.spec_skills_for_basic_training, this.has_AE, specializations, this.serviceTable_Renamed.ToArray(), this.personalDevelopment.ToArray(), this.officerTable_Renamed.Count == 0 ? (Outcome[]) null : this.officerTable_Renamed.ToArray(), this.advancedEducationTable_Renamed.Count == 0 ? (Outcome[]) null : this.advancedEducationTable_Renamed.ToArray(), this.events_Renamed, this.mishaps_Renamed, this.officerRanks_Renamed == null || this.officerRanks_Renamed.Count == 0 ? (Dictionary<int, Rank>) null : this.officerRanks_Renamed, this.careerRanks_Renamed, this.mobBenefits);
      if (this.requiredTraits.Count > 0)
      {
        foreach (Trait requiredTrait in this.requiredTraits)
          career.addRequiredTrait(requiredTrait);
      }
      if (this.qualMods.Count > 0)
      {
        foreach (CareerQualModifier qualMod in this.qualMods)
          career.addQualModifier(qualMod);
      }
      if (this.qualChallenges.Count > 0)
      {
        foreach (CareerChallenge qualChallenge in this.qualChallenges)
          career.addQualChallenges(qualChallenge);
      }
      if (this.qualFilters.Count > 0)
      {
        foreach (CareerQualFilter qualFilter in this.qualFilters)
          career.addQualFilter(qualFilter);
      }
      if (this.psionicsTable_Ranamed != null && this.psionicsTable_Ranamed.Any<Outcome>())
      {
        career.SocLevelRequiredForPsionics = this._socLevelForPsionics;
        career.PsionicSkills = (IList<Outcome>) this.psionicsTable_Ranamed.ToArray();
      }
      career.AutomaticCommissionIfStatIsAtOrOverTarget = this.automaticCommissionIfStatIsAtOrOverTarget;
      career.CareerFinishedOnFailedAssignmentChange = this.CareerFinishedOnFailedAssignmentChange == CareerBuilder.TristateBoolean.TRUE;
      return career;
    }

    private enum TristateBoolean
    {
      NOT_SET,
      TRUE,
      FALSE,
    }

    public class EquipementMusteringOutBenefit : OutcomeOperator
    {
      public void operateOnOutcome(Outcome outcome)
      {
      }
    }
  }
}
