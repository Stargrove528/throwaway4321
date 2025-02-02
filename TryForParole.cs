
// Type: com.digitalarcsystems.Traveller.DataModel.TryForParole




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class TryForParole(string title, string description) : Event(title, description)
  {
    public override void handleOutcome(GenerationState currentState)
    {
      if (currentState.currentCareer is CaptiveCareer)
      {
        List<Outcome> outcomeList = ((CaptiveCareer) currentState.currentCareer).tryForParole(0, currentState.character, true);
        currentState.recorder.RecordBenefit((Event) this, currentState);
        foreach (Outcome outcome in outcomeList)
          outcome.handleOutcome(currentState);
      }
      else
        EngineLog.Error("TryForParole event called on non CaptiveCareer [" + currentState.currentCareer.Name + "]");
    }
  }
}
