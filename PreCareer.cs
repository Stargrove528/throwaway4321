
// Type: com.digitalarcsystems.Traveller.DataModel.PreCareer




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class PreCareer : Career
  {
    [JsonIgnore]
    public const string PRECAREER_CATEGORY = "PRECAREER";
    public int honors = 11;
    public int graduation = 7;
    public string graduationStat = "Int";

    public bool HonorsBenefitsReplaceGradBenefits { set; get; }

    [JsonConstructor]
    public PreCareer()
    {
      if (this.Category != null && this.Category.Length > 0)
        this.Category += ", ";
      else
        this.Category = "";
      this.Category += "PRECAREER";
    }

    private static Dictionary<int, Outcome> convertListToDictionary(List<Outcome> convertMe)
    {
      Dictionary<int, Outcome> dictionary = new Dictionary<int, Outcome>();
      int num = 1;
      if (convertMe.Count > 6)
        num = 0;
      for (int index = 0; index < convertMe.Count; ++index)
        dictionary[index + num] = convertMe[index];
      return dictionary;
    }

    public PreCareer(
      string name,
      string description,
      string category,
      string qual_stat_name,
      int qual_target,
      int modifer_per_term_not_enrolled,
      int max_num_terms_before_entry,
      string grad_stat_name,
      int grad_target,
      int honor_target,
      List<Outcome> enrollment_outcomes,
      Dictionary<int, Event> events,
      List<Outcome> graduation_benefits,
      List<Outcome> honors_benefits)
      : base(name, description, modifer_per_term_not_enrolled, 18 + 4 * max_num_terms_before_entry, -100, 0, category, qual_stat_name, qual_target, false, "", 100, false, false, (Career.Specialization[]) null, enrollment_outcomes.ToArray(), graduation_benefits.ToArray(), (Outcome[]) null, honors_benefits.ToArray(), events, (Dictionary<int, Event>) null, (Dictionary<int, Rank>) null, (Dictionary<int, Rank>) null, (Dictionary<int, MusteringOutBenefit>) null)
    {
      this.Description = description;
      this.addQualFilter((CareerQualFilter) new MaxNumTermsCareerQualFilter(max_num_terms_before_entry));
    }

    public override IList<Outcome> qualifyForAdvancement(
      Character character,
      string specialization,
      int modifier = 0)
    {
      List<Outcome> outcomeList = new List<Outcome>();
      string str = "";
      if (modifier != 0)
        str = modifier <= 0 ? "It looks like things may be stacked against you. You're definitely swimming upstream. Good luck!" : "Your chances are improved due to your gained experience or status. Good luck!";
      RollEffect rollEffect = Dice.Roll(new RollParam(character.getAttribute(this.graduationStat), this.graduation, modifier, "Not everyone graduates from " + this.Name + ". Will you? " + str, RollType.NORMAL));
      if (rollEffect.isSuccessful)
      {
        if (rollEffect.totalResult >= this.honors)
        {
          outcomeList.Add((Outcome) new Event("Graduated with Honors!", "You proved yourself one of the best of the best. You graduated from the " + this.Name + " with Honors! The sky's the limit."));
          outcomeList.AddRange((IEnumerable<Outcome>) this.honorsBenefits());
          if (!this.HonorsBenefitsReplaceGradBenefits)
            outcomeList.AddRange((IEnumerable<Outcome>) this.graduationBenefits());
        }
        else
        {
          outcomeList.Add((Outcome) new Event("Graduated!", "You made it! You graduated from the " + this.Name + " and the future looks bright!"));
          outcomeList.AddRange((IEnumerable<Outcome>) this.graduationBenefits());
        }
      }
      else
        outcomeList.Add((Outcome) new Event("Washed out of the " + this.Name, "You just weren't meant to graduation from the " + this.Name + ". You pack your things and leave school without a diploma. But you will not leave completely empty handed: you've gained some years and some knowledge."));
      return (IList<Outcome>) outcomeList;
    }

    public List<Outcome> enrollmentBenefits()
    {
      return new List<Outcome>((IEnumerable<Outcome>) this.ServiceSkills);
    }

    public List<Outcome> graduationBenefits()
    {
      return new List<Outcome>((IEnumerable<Outcome>) this.PersonalDevelopment);
    }

    public List<Outcome> honorsBenefits() => this.AdvancedEducationList;
  }
}
