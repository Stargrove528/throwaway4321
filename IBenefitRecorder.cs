
// Type: com.digitalarcsystems.Traveller.IBenefitRecorder




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IBenefitRecorder
  {
    void RecordBenefit(Outcome outcome, GenerationState currentState);

    void RecordBenefit(Event aevent, GenerationState currentState);
  }
}
