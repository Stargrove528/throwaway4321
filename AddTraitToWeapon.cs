
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.AddTraitToWeapon




using Newtonsoft.Json;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class AddTraitToWeapon : IAmmoAction
  {
    public string TraitToAdd { get; set; }

    public string ValueToAdd { get; set; }

    private EquipmentExtensions.Trait WhatWeAdded { get; set; }

    private EquipmentExtensions.Trait WhatWeRemoved { get; set; }

    public bool only_add_if_it_doesnt_already_exist { get; set; }

    public string AdditionalInfo { get; set; }

    [JsonConstructor]
    public AddTraitToWeapon()
    {
    }

    public virtual void ActionsOnAdd(IMultishotRangedWeapon addedToThis)
    {
      if (addedToThis.Traits == null)
        addedToThis.Traits = "";
      if (!this.only_add_if_it_doesnt_already_exist || this.only_add_if_it_doesnt_already_exist && !addedToThis.Traits.ToLower().Contains(this.TraitToAdd.ToLower()))
      {
        this.WhatWeRemoved = addedToThis.GetTrait(this.TraitToAdd);
        if (this.ValueToAdd == null)
          this.ValueToAdd = "";
        this.WhatWeAdded = new EquipmentExtensions.Trait(this.TraitToAdd + (this.TraitToAdd.Last<char>() == ' ' ? "" : " ") + this.ValueToAdd);
        addedToThis.addTrait(this.WhatWeAdded);
      }
      else
        this.WhatWeAdded = (EquipmentExtensions.Trait) null;
    }

    public virtual void ActionsOnRemoval(IMultishotRangedWeapon removedFromThis)
    {
      if (this.WhatWeAdded == null)
        return;
      removedFromThis.removeTrait(this.WhatWeAdded);
      if (this.WhatWeRemoved != null)
        removedFromThis.addTrait(this.WhatWeRemoved);
    }
  }
}
