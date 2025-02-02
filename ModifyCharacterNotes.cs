
// Type: com.digitalarcsystems.Traveller.DataModel.Events.ModifyCharacterNotes




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class ModifyCharacterNotes : Event
  {
    public string add2Notes { get; set; }

    public ModifyCharacterNotes()
    {
    }

    public ModifyCharacterNotes(string name, string description)
      : base(name, description)
    {
      this.add2Notes = description;
    }

    public ModifyCharacterNotes(string addToNotes) => this.add2Notes = addToNotes;

    public override void handleOutcome(GenerationState currentState)
    {
      currentState.character.Notes = (currentState.character.Notes += this.add2Notes);
      base.handleOutcome(currentState);
    }
  }
}
