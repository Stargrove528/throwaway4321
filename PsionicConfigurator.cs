
// Type: com.digitalarcsystems.Traveller.PsionicConfigurator




using com.digitalarcsystems.Traveller.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class PsionicConfigurator(ICharacterCreationAlgorithm engine) : Configurator(engine)
  {
    public override void processBeforeNextOperation(GenerationState currentState)
    {
      if (!this.RequiresProcessBeforeNextOperation || currentState.nextOperation != CreationOperation.SELECT_RACE)
        return;
      List<com.digitalarcsystems.Traveller.DataModel.Attribute> attributeList = new List<com.digitalarcsystems.Traveller.DataModel.Attribute>((IEnumerable<com.digitalarcsystems.Traveller.DataModel.Attribute>) currentState.character.getAttributes());
      if (!attributeList.Any<com.digitalarcsystems.Traveller.DataModel.Attribute>((Func<com.digitalarcsystems.Traveller.DataModel.Attribute, bool>) (a => a.Name.Equals(Trait.PSIONIC.associatedAttribute, StringComparison.InvariantCulture))))
      {
        com.digitalarcsystems.Traveller.DataModel.Attribute attribute = new com.digitalarcsystems.Traveller.DataModel.Attribute(Trait.PSIONIC.associatedAttribute, 6, 2, 0);
        attributeList.Add(attribute);
        currentState.character.setAttributes(attributeList);
        currentState.character.addTrait(Trait.PSIONIC);
      }
    }

    public override Configuration generateDefaultConfiguration()
    {
      return new Configuration()
      {
        ConfiguratorName = this.Name,
        Name = "Start Off Psionic",
        Description = "By setting this true, each character will start with a Psi stat, and Psionic Trait.  This will allow generated characters to take the Psion Career.",
        CreatingUser = DataManager.UserID,
        Id = new Guid("e630d342-1fc4-43bc-9622-6486aa274773")
      };
    }

    public override void handleConfiguration(Configuration handleMe)
    {
      if (!handleMe.ConfiguratorName.Equals(this.Name, StringComparison.InvariantCulture))
        return;
      this._enabled = handleMe.asBoolean();
      this.RequiresProcessBeforeNextOperation = this._enabled;
    }
  }
}
