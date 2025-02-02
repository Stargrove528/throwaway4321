
// Type: com.digitalarcsystems.Traveller.DataModel.NoParoleRole




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class NoParoleRole : Event
  {
    public override void handleOutcome(GenerationState currentState)
    {
      if (currentState.currentCareer is CaptiveCareer)
        ((CaptiveCareer) currentState.currentCareer).NoRole = true;
      else
        EngineLog.Error("ParoleModifier Event, but career wasn't Captive Career [" + currentState.currentCareer.Name + "]");
    }
  }
}
