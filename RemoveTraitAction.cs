
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.RemoveTraitAction




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class RemoveTraitAction : IEquipmentAction, IEquipmentModifier, IModifier<IEquipment>
  {
    [JsonProperty]
    private int index_where_trait_was_found = -1;

    [JsonProperty]
    public string TraitToRemove { get; set; }

    private EquipmentExtensions.Trait WhatIRemoved { get; set; }

    public string AdditionalInfo { get; set; }

    public void ActionsOnAdd(IEquipment addedToThis)
    {
      this.WhatIRemoved = addedToThis.GetTrait(this.TraitToRemove);
      addedToThis.removeTrait(this.TraitToRemove);
    }

    public void ActionsOnRemoval(IEquipment removedFromThis)
    {
      if (this.WhatIRemoved == null)
        return;
      removedFromThis.addTrait(this.WhatIRemoved);
      this.WhatIRemoved = (EquipmentExtensions.Trait) null;
    }
  }
}
