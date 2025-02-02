
// Type: com.digitalarcsystems.Traveller.DataModel.CaptiveCareer




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class CaptiveCareer : Career
  {
    public string release_threshold_label = "parole";
    private bool _justEntered = true;

    public int ReleaseThreshold { get; set; }

    public bool NoRole { get; set; }

    [JsonConstructor]
    public CaptiveCareer() => this.AllowUseOfAnagathics = false;

    public CaptiveCareer(int release_threshold, Career career)
      : base(career)
    {
      this.AllowUseOfAnagathics = false;
      this.ReleaseThreshold = release_threshold;
      this.NoRole = false;
    }

    public CaptiveCareer(int release_threshold, Career career, string name, string description)
      : this(release_threshold, career)
    {
      this.Name = name;
      this.Description = description;
    }

    public void initializeCaptiveCareer()
    {
      this.ReleaseThreshold = Dice.D6Roll(1, 4, "Unlike other careers, to get out of " + this.Name + ", your advancement roll must exceed your " + this.release_threshold_label + " every term.  Roll to determine your initial " + this.release_threshold_label + " value.  (Lower is better.)").totalResult;
    }

    public override IList<Career.Specialization> GetApplicableSpecializations(Character character)
    {
      if (this._justEntered)
      {
        this.initializeCaptiveCareer();
        this._justEntered = false;
      }
      return base.GetApplicableSpecializations(character);
    }

    public override IList<Career.Specialization> Specializations
    {
      get
      {
        if (this._justEntered)
        {
          this.initializeCaptiveCareer();
          this._justEntered = false;
        }
        return base.Specializations;
      }
    }

    public override IList<Outcome> qualifyForAdvancement(
      Character character,
      string specialization,
      int modifier = 0)
    {
      IList<Outcome> outcomeList = this.tryForAdvancement(character, specialization, false, modifier);
      if (this.ReleaseThreshold > 12)
        this.ReleaseThreshold = 12;
      if (this.NoRole)
      {
        this.NoRole = false;
      }
      else
      {
        foreach (Outcome outcome in this.tryForParole(this.currentAdvancementRollEffect.totalResult, character))
          outcomeList.Add(outcome);
      }
      return outcomeList;
    }

    public virtual List<Outcome> tryForParole(
      int value_or_modifier,
      Character character,
      bool roll = false)
    {
      List<Outcome> outcomeList = new List<Outcome>();
      int num = value_or_modifier;
      if (roll)
        num = Dice.D6Roll(2, value_or_modifier, "Rolling for Parole").totalResult;
      if (num >= this.ReleaseThreshold)
      {
        Event.EndOfCareer endOfCareer = new Event.EndOfCareer("Congratulations!  After " + (character.CurrentTerm.numberOfTermsInCareerIncludingThisOne * 4).ToString() + " years you made " + this.release_threshold_label + ". At the end of this term you will be released.");
        outcomeList.Add((Outcome) endOfCareer);
        outcomeList.Add((Outcome) new Outcome.InformUser("You Make Parole!", endOfCareer.Description));
        this._justEntered = true;
      }
      else
      {
        Event.MustContinueCareer mustContinueCareer = new Event.MustContinueCareer("Still in " + this.Name, "You don't make " + this.release_threshold_label + ".  Your Advancement was [" + num.ToString() + "] and your " + this.release_threshold_label + " was " + this.ReleaseThreshold.ToString() + ".  Your new " + this.release_threshold_label + " is [" + (--this.ReleaseThreshold).ToString() + "].");
        outcomeList.Add((Outcome) mustContinueCareer);
        outcomeList.Add((Outcome) new Outcome.InformUser("Still in " + this.Name, mustContinueCareer.Description));
      }
      return outcomeList;
    }
  }
}
