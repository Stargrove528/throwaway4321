
// Type: com.digitalarcsystems.Traveller.DataModel.Events.GainIndependence




using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class GainIndependence : Event
  {
    public GainIndependence()
      : base("Independence", "The Traveller is capable of dealing with the concepts of money. In Aslan society, males rarely understand such matters, a fact which handicaps them in a technological society.")
    {
    }

    public GainIndependence(string name, string description)
      : base(name, description)
    {
    }

    public override void handleOutcome(GenerationState currentState)
    {
      if (Dice.TwoDiceRollEffect(currentState.character.getAttributeValue("SOC"), "Attempt to gain a level of independence.  Must beat your Soc.", "[SUCCEEDED] You did it!  Math and money isn't just for girls.", "[FAILED] Ugh.  Who needs math anyway.  It's women's work.") >= 0)
      {
        currentState.recorder.RecordBenefit((Event) this, currentState);
        new Outcome.GainSkill("Independence").handleOutcome(currentState);
      }
      else
      {
        this.Description += " [FAILED]";
        base.handleOutcome(currentState);
      }
    }

    public override string ToString()
    {
      string source = this._name != null ? this._name : "";
      if (!source.Any<char>())
        source = "Independence";
      return source.ToString();
    }
  }
}
