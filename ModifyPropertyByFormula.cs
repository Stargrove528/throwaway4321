
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ModifyPropertyByFormula




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ModifyPropertyByFormula : IAmmoAction
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
    public ModifyPropertyByFormula()
    {
    }

    public void ActionsOnAdd(IMultishotRangedWeapon addedToThis)
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

    public void ActionsOnRemoval(IMultishotRangedWeapon removedFromThis)
    {
      removedFromThis.GetType().GetProperty(this.Property).SetValue((object) removedFromThis, this.previousValue, (object[]) null);
    }
  }
}
