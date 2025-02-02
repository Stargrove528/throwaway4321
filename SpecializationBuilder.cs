
// Type: com.digitalarcsystems.Traveller.SpecializationBuilder




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class SpecializationBuilder
  {
    private static readonly int DIDNT_CONVERT = -2147483647;
    private Career.Specialization specialization;
    private string name_Renamed = (string) null;
    private string description_Renamed = (string) null;
    private int survival = int.MinValue;
    private string survivalStat_Renamed = (string) null;
    private int advancement = int.MinValue;
    private string advancementStat_Renamed = (string) null;
    private Dictionary<int, Rank> ranks = new Dictionary<int, Rank>();
    private List<Rank> initialRanks = new List<Rank>();
    private List<Outcome> skills = new List<Outcome>();
    private List<CareerQualFilter> specializationFilters = new List<CareerQualFilter>();

    public virtual SpecializationBuilder name(string name)
    {
      this.name_Renamed = name;
      return this;
    }

    public virtual SpecializationBuilder survivalTarget(int surv_target)
    {
      this.survival = surv_target;
      return this;
    }

    public virtual SpecializationBuilder survivalTarget(string surv_target)
    {
      int result;
      this.survival = int.TryParse(surv_target, out result) ? result : SpecializationBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual SpecializationBuilder advancementTarget(int adv_target)
    {
      this.advancement = adv_target;
      return this;
    }

    public virtual SpecializationBuilder advancementTarget(string adv_target)
    {
      int result;
      this.advancement = int.TryParse(adv_target, out result) ? result : SpecializationBuilder.DIDNT_CONVERT;
      return this;
    }

    public virtual SpecializationBuilder addQualFilter(CareerQualFilter filter)
    {
      this.specializationFilters.Add(filter);
      return this;
    }

    public virtual SpecializationBuilder description(string specializationDescription)
    {
      this.description_Renamed = specializationDescription;
      return this;
    }

    public virtual SpecializationBuilder survivalStat(string survStat)
    {
      this.survivalStat_Renamed = survStat;
      return this;
    }

    public virtual SpecializationBuilder advancementStat(string advStat)
    {
      this.advancementStat_Renamed = advStat;
      return this;
    }

    public virtual SpecializationBuilder addSkill(Outcome outcome)
    {
      this.skills.Add(outcome);
      return this;
    }

    public virtual SpecializationBuilder addRank(Rank rank)
    {
      this.initialRanks.Add(rank);
      return this;
    }

    public virtual IList<ValidationResult> validate()
    {
      IList<string> canonicalStats = (IList<string>) Attribute.CanonicalStats;
      List<ValidationResult> validationResultList = new List<ValidationResult>();
      if (this.name_Renamed == null || this.name_Renamed.Length == 0)
        validationResultList.Add(ValidationResult.error("Specialization must have a valid name.  It can't be null or empty"));
      if (this.description_Renamed == null || this.description_Renamed.Length == 0)
        validationResultList.Add(ValidationResult.warning("Specializations should have descriptions."));
      if (this.survivalStat_Renamed == null || this.survivalStat_Renamed.Length == 0)
        validationResultList.Add(ValidationResult.error("Specialization survival stat must be defined"));
      else if (!canonicalStats.Contains(this.survivalStat_Renamed))
        validationResultList.Add(ValidationResult.warning("Specialization SurvivalStat [" + this.survivalStat_Renamed + "] isn't one of the canonical stats."));
      if (this.survival <= 0)
        validationResultList.Add(ValidationResult.error("The specialization survival target must be a valid positive number."));
      if (this.advancementStat_Renamed == null || this.advancementStat_Renamed.Length == 0)
        validationResultList.Add(ValidationResult.error("Specialization Advancement Stat must be defined"));
      else if (!canonicalStats.Contains(this.advancementStat_Renamed))
        validationResultList.Add(ValidationResult.warning("Specialization AdvancementStat [" + this.advancementStat_Renamed + "] isn't one of the canonical stats."));
      if (this.advancement <= 0)
        validationResultList.Add(ValidationResult.error("The specialization advancement target must be a valid positive number."));
      IList<int> intList = (IList<int>) new List<int>();
      foreach (Rank initialRank in this.initialRanks)
      {
        if (intList.Contains(initialRank.level))
          validationResultList.Add(ValidationResult.error("Specialization Rank [" + initialRank.level.ToString() + "] is duplicated.  Rank levels must be unique."));
      }
      if (this.skills.Count != 6)
        validationResultList.Add(ValidationResult.error("There must be at least 6 outcomes in a Career Specialization Skill Table.  Found [" + this.skills.Count.ToString() + "]"));
      return (IList<ValidationResult>) validationResultList;
    }

    public virtual Career.Specialization createSpecialization()
    {
      this.ranks.Clear();
      foreach (Rank initialRank in this.initialRanks)
        this.ranks.Add(initialRank.level, initialRank);
      this.specialization = new Career.Specialization(this.name_Renamed, this.description_Renamed, this.survivalStat_Renamed, this.survival, this.advancementStat_Renamed, this.advancement, this.skills.ToArray(), this.ranks);
      if (this.specializationFilters.Any<CareerQualFilter>())
      {
        foreach (CareerQualFilter specializationFilter in this.specializationFilters)
          this.specialization.AddQualFilter(specializationFilter);
      }
      return this.specialization;
    }
  }
}
