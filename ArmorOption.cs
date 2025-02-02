
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ArmorOption




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ArmorOption : FixedPriceEquipmentOption, IModifier<Armor>
  {
    [JsonProperty]
    private new List<IEquipmentAction> _actions = new List<IEquipmentAction>();

    [JsonConstructor]
    public ArmorOption(List<IEquipmentAction> actions) => this._actions = actions;

    public ArmorOption()
    {
    }

    void IModifier<Armor>.ActionsOnAdd(Armor addedToThis)
    {
      foreach (IModifier<IEquipment> action in this._actions)
        action.ActionsOnAdd((IEquipment) addedToThis);
    }

    void IModifier<Armor>.ActionsOnRemoval(Armor removedFromThis)
    {
      foreach (IModifier<IEquipment> action in this._actions)
        action.ActionsOnRemoval((IEquipment) removedFromThis);
    }
  }
}
