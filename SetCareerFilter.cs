
// Type: com.digitalarcsystems.Traveller.DataModel.Events.SetCareerFilter




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class SetCareerFilter : Event
  {
    [JsonProperty]
    public ICareerFilter newCareerFilter;

    [JsonConstructor]
    public SetCareerFilter()
    {
    }

    public SetCareerFilter(ICareerFilter newCareerFilter) => this.newCareerFilter = newCareerFilter;

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.careerSource.SetCareerFilter(this.newCareerFilter);
      base.handleOutcome(currentState);
    }
  }
}
