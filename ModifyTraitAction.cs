
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ModifyTraitAction




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ModifyTraitAction : IEquipmentAction, IEquipmentModifier, IModifier<IEquipment>
  {
    public string TraitToAdd { get; set; }

    public string ValueToAdd { get; set; }

    private EquipmentExtensions.Trait WhatWeAdded { get; set; }

    private EquipmentExtensions.Trait WhatWeRemoved { get; set; }

    public bool only_add_if_it_doesnt_already_exist { get; set; }

    public string AdditionalInfo { get; set; }

    [JsonConstructor]
    public ModifyTraitAction()
    {
    }

    public virtual void ActionsOnAdd(IEquipment addedToThis)
    {
      if (addedToThis.Traits == null)
        addedToThis.Traits = "";
      if (!this.only_add_if_it_doesnt_already_exist || this.only_add_if_it_doesnt_already_exist && !addedToThis.Traits.ToLower().Contains(this.TraitToAdd.ToLower()))
      {
        List<EquipmentExtensions.Trait> traits = addedToThis.GetTraits();
        if (traits.Count<EquipmentExtensions.Trait>() > 0 && traits.Any<EquipmentExtensions.Trait>((Func<EquipmentExtensions.Trait, bool>) (t => t.name.ToLowerInvariant().Contains(this.TraitToAdd.ToLowerInvariant()))))
        {
          for (int index = 0; index < traits.Count<EquipmentExtensions.Trait>(); ++index)
          {
            if (traits.ElementAt<EquipmentExtensions.Trait>(index).name.ToLowerInvariant().Equals(this.TraitToAdd.ToLowerInvariant()))
            {
              this.WhatWeRemoved = traits.ElementAt<EquipmentExtensions.Trait>(index);
              addedToThis.removeTrait(this.WhatWeRemoved);
              break;
            }
          }
        }
        this.WhatWeAdded = new EquipmentExtensions.Trait(this.TraitToAdd + (this.ValueToAdd != null ? " " + this.ValueToAdd : ""));
        addedToThis.addTrait(this.WhatWeAdded);
      }
      else
        this.WhatWeAdded = (EquipmentExtensions.Trait) null;
    }

    public virtual void ActionsOnRemoval(IEquipment removedFromThis)
    {
      if (this.WhatWeAdded == null)
        return;
      removedFromThis.removeTrait(this.WhatWeAdded);
      this.WhatWeAdded = (EquipmentExtensions.Trait) null;
      if (this.WhatWeRemoved != null)
      {
        removedFromThis.addTrait(this.WhatWeRemoved);
        this.WhatWeRemoved = (EquipmentExtensions.Trait) null;
      }
    }
  }
}
