
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.AbstractEquipmentOption




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public abstract class AbstractEquipmentOption : 
    com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment,
    IEquipmentOption,
    IEquipmentModifier,
    IModifier<IEquipment>,
    IEquipment,
    ICustomDescribable,
    IDescribable,
    IAsset,
    IAssetBase,
    ICategorizable
  {
    [JsonProperty]
    protected List<IEquipmentAction> _actions = new List<IEquipmentAction>();

    [JsonProperty]
    public List<string> UpgradeCategories { get; set; }

    [JsonConstructor]
    public AbstractEquipmentOption()
    {
    }

    public AbstractEquipmentOption(IEquipment copyMe)
      : base(copyMe)
    {
    }

    public AbstractEquipmentOption(AbstractEquipmentOption copyMe)
      : base((IEquipment) copyMe)
    {
      this._actions = copyMe != null ? copyMe._actions : throw new ArgumentException("copyMe can't be null!");
      this.UpgradeCategories = new List<string>((IEnumerable<string>) copyMe.UpgradeCategories);
      if (this._actions != null)
        return;
      this._actions = new List<IEquipmentAction>();
      EngineLog.Warning("AbstractEquipmentOption copy constructor found null copyMe._actions");
    }

    public virtual void ActionsOnAdd(IEquipment addedToThis)
    {
      if (!this._actions.Any<IEquipmentAction>())
      {
        IEquipment equipment = addedToThis;
        equipment.Description = equipment.Description + "\n[CONFIGURED OPTION: (" + this.Name + ") " + this.Description + "]";
      }
      else
      {
        foreach (IEquipmentAction action in this._actions)
        {
          if (action != null)
            action.ActionsOnAdd(addedToThis);
          else
            EngineLog.Warning("Null equipment action configured in " + this.Name);
        }
      }
    }

    public virtual void ActionsOnRemoval(IEquipment removedFromThis)
    {
      if (!this._actions.Any<IEquipmentAction>())
      {
        removedFromThis.Description = removedFromThis.Description.Replace("\n[CONFIGURED OPTION: (" + this.Name + ") " + this.Description + "]", "");
      }
      else
      {
        foreach (IEquipmentAction action in this._actions)
        {
          if (action != null)
            action.ActionsOnRemoval(removedFromThis);
          else
            EngineLog.Warning("Null equipment action configured in " + this.Name);
        }
      }
    }

    public abstract int CalculatePrice(IEquipment equipmentToBeAddedTo);

    public virtual void AddEquipmentAction(IEquipmentAction addMe) => this._actions.Add(addMe);
  }
}
