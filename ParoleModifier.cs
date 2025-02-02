
// Type: com.digitalarcsystems.Traveller.DataModel.ParoleModifier




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class ParoleModifier : Event
  {
    public const int NO_ROLL = -100;

    public int Modifier { get; set; }

    public ParoleModifier(string title, string description, int modifier)
      : base(title, description)
    {
      this.Modifier = modifier;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      if (currentState.currentCareer is CaptiveCareer)
      {
        CaptiveCareer currentCareer = (CaptiveCareer) currentState.currentCareer;
        if (this.Modifier == -100)
        {
          currentCareer.NoRole = true;
        }
        else
        {
          currentCareer.ReleaseThreshold += this.Modifier;
          currentState.recorder.RecordBenefit((Event) this, currentState);
        }
      }
      else
        EngineLog.Error("ParoleModifier Event, but career wasn't Captive Career [" + currentState.currentCareer.Name + "]");
    }
  }
}
