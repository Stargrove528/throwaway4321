
// Type: com.digitalarcsystems.Traveller.DataModel.Events.AddYearsToCharacter




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class AddYearsToCharacter : Outcome
  {
    private int years_to_add;

    public AddYearsToCharacter(int numYears) => this.years_to_add = numYears;

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.character.Age += this.years_to_add;
    }
  }
}
