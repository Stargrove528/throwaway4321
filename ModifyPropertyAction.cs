
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ModifyPropertyAction




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Reflection;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ModifyPropertyAction : IEquipmentAction, IEquipmentModifier, IModifier<IEquipment>
  {
    [JsonProperty]
    public string Property { get; set; }

    [JsonProperty]
    public string Formula { get; set; }

    [JsonProperty]
    public object previousValue { get; set; }

    [JsonProperty]
    public string AdditionalInfo { get; set; }

    [JsonConstructor]
    public ModifyPropertyAction()
    {
    }

    public void ActionsOnAdd(IEquipment addedToThis)
    {
      if (this.Property.ToLowerInvariant().Equals("exponentiator"))
        this.Property = "DamageExponentiator";
      try
      {
        this.previousValue = addedToThis.GetType().GetProperty(this.Property).GetValue((object) addedToThis, (object[]) null);
        addedToThis.GetType().GetProperty(this.Property).SetValue((object) addedToThis, (object) Utility.calculateValue(this.Formula, (object) addedToThis), (object[]) null);
      }
      catch (Exception ex)
      {
        EngineLog.Error("ModifyPropertyAction unable to modify [" + this.Property + "] on " + (addedToThis != null ? addedToThis.Name : "NULL") + ":\n" + ex.Message + "\n" + ex.StackTrace);
      }
    }

    public void ActionsOnRemoval(IEquipment removedFromThis)
    {
      if (!(removedFromThis.GetType().GetProperty(this.Property) != (PropertyInfo) null))
        return;
      removedFromThis.GetType().GetProperty(this.Property).SetValue((object) removedFromThis, this.previousValue, (object[]) null);
    }
  }
}
