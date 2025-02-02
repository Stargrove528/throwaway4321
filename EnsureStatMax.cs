
// Type: com.digitalarcsystems.Traveller.DataModel.Events.EnsureStatMax




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class EnsureStatMax : Outcome
  {
    public string stat_name;
    public int maximum_value;

    public EnsureStatMax(string statName, int maximumValue)
    {
      this.stat_name = statName;
      this.maximum_value = maximumValue;
    }

    public override void handleOutcome(GenerationState currentState)
    {
      Attribute attribute = currentState.character.getAttribute(this.stat_name);
      if (attribute.Value > this.maximum_value || attribute.UninjuredValue > this.maximum_value)
      {
        attribute.UninjuredValue = this.maximum_value;
        attribute.Value = this.maximum_value;
      }
      currentState.recorder.RecordBenefit((Outcome) this, currentState);
    }
  }
}
