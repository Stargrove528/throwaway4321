
// Type: com.digitalarcsystems.Traveller.DataModel.MilitaryAcademy




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class MilitaryAcademy : PreCareer
  {
    public MilitaryAcademy()
    {
    }

    public MilitaryAcademy(
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
      : base(name, description, category, qual_stat_name, qual_target, modifer_per_term_not_enrolled, max_num_terms_before_entry, grad_stat_name, grad_target, honor_target, enrollment_outcomes, events, graduation_benefits, honors_benefits)
    {
    }

    public override IList<Outcome> qualifyForAdvancement(
      Character character,
      string specialization,
      int modifier = 0)
    {
      string str = "";
      if (character.getAttribute("End").Value >= 8)
      {
        ++modifier;
        str = "A high endurance really helps keep you pushing hard against the obsticals (+1 to graduate). ";
      }
      if (character.getAttribute("Soc").Value >= 8)
      {
        ++modifier;
        str += " A high social standing means more help when you need it, and more attention by your teachers (+1 to graduate).";
      }
      if (modifier > 0)
        FeedbackStream.Send("Graudating the academy is a little easier for you. " + str);
      return base.qualifyForAdvancement(character, specialization, modifier);
    }
  }
}
