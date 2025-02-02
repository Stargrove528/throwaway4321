
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ModifyDescriptionAction




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ModifyDescriptionAction : IEquipmentAction, IEquipmentModifier, IModifier<IEquipment>
  {
    public string DescriptionToAdd { get; set; }

    protected string WhatWeAdded { get; set; }

    void IModifier<IEquipment>.ActionsOnAdd(IEquipment addedToThis)
    {
      this.WhatWeAdded = "\n" + this.DescriptionToAdd;
      addedToThis.Description += this.WhatWeAdded;
    }

    void IModifier<IEquipment>.ActionsOnRemoval(IEquipment removedFromThis)
    {
      int startIndex = removedFromThis.Description.LastIndexOf(this.WhatWeAdded, StringComparison.InvariantCultureIgnoreCase);
      if (startIndex < 0)
        return;
      removedFromThis.Description = removedFromThis.Description.Remove(startIndex, this.WhatWeAdded.Length);
    }
  }
}
