
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Membership




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Membership : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
  {
    [JsonProperty]
    public List<Outcome> benefits;

    private Membership()
    {
    }

    [JsonConstructor]
    public Membership(Guid instanceID)
    {
      this.InstanceID = instanceID;
      if (!(instanceID == Guid.Empty))
        return;
      this.InstanceID = Guid.NewGuid();
    }

    public Membership(Guid membership_id, string name, string description)
    {
      this.Name = name;
      this.Description = description;
      this.Id = membership_id;
      this.InstanceID = Guid.NewGuid();
      this.Categories = new List<string>() { "General" };
      this.Categories.AddDistinct<string>("Organization");
    }

    public Membership(
      Guid membership_id,
      string name,
      string description,
      params Outcome[] benefits)
      : this(membership_id, name, description)
    {
      this.benefits = new List<Outcome>((IEnumerable<Outcome>) benefits);
    }

    public override void handleOutcome(GenerationState currentState)
    {
      this.InstanceID = Guid.NewGuid();
      if (this.benefits != null && this.benefits.Any<Outcome>())
      {
        foreach (Outcome benefit in this.benefits)
          benefit.handleOutcome(currentState);
      }
      Character character = currentState.character;
      character.Notes = character.Notes + "\n=============== Membership ===============\nName: " + this.Name + "\nDescription:\n\t" + this.Description + "\n===========================================\n";
      base.handleOutcome(currentState);
    }
  }
}
