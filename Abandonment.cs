
// Type: com.digitalarcsystems.Traveller.DataModel.Events.Abandonment




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Events
{
  public class Abandonment : Event
  {
    [JsonProperty]
    public Contact abandoner;

    [JsonConstructor]
    public Abandonment()
    {
    }

    public Abandonment(string name, string description)
      : base(name, description)
    {
    }

    public override void handleOutcome(GenerationState currentState)
    {
      List<Contact> list = currentState.character.EntityIKnow.Where<Contact>((Func<Contact, bool>) (c => c.Type == Contact.ContactType.Ally || c.Type == Contact.ContactType.Contact)).ToList<Contact>();
      if (list.Any<Contact>())
      {
        this.abandoner = list.Count != 1 ? currentState.decisionMaker.ChooseOne<Contact>("Which Contact or Ally abandoned you?", (IList<Contact>) list) : list.First<Contact>();
        currentState.character.removeEntityIKnow(this.abandoner);
      }
      else
        new Outcome.MusteringOutBenefitModifier(-1).handleOutcome(currentState);
    }

    public override string ToString()
    {
      return "Abondoned. " + (this.abandoner != null ? this.abandoner.Name + " left you." : "You lost all benefits from this term.");
    }
  }
}
