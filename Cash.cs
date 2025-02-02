
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Cash




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Cash : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
  {
    public int Amount = 0;

    [JsonConstructor]
    public Cash()
    {
    }

    public Cash(int amount)
    {
      this.Name = amount.ToString() + " credits";
      this.Amount = amount;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.character.Credits += this.Amount;
      if (currentState.nextOperation == CreationOperation.GENERATE_MUSTERING_OUT_BENEFIT)
        return;
      currentState.recorder.RecordBenefit((Outcome) this, currentState);
    }
  }
}
