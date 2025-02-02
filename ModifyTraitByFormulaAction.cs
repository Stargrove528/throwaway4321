
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ModifyTraitByFormulaAction




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ModifyTraitByFormulaAction : ModifyTraitAction
  {
    [JsonProperty]
    public string Formula { get; set; }

    [JsonConstructor]
    public ModifyTraitByFormulaAction()
    {
    }

    public new void ActionsOnAdd(IEquipment addedToThis)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (EquipmentExtensions.Trait trait in addedToThis.GetTraits())
      {
        string key = trait.name.Trim();
        string str = trait.has_value ? trait.value.ToString() ?? "" : "";
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, str);
      }
      List<string> list = Utility.tokenize(this.Formula).Where<string>((Func<string, bool>) (tg => tg.Contains("Trait:"))).Select<string, string>((Func<string, string>) (tg => tg.Split(':')[1])).ToList<string>();
      string formula = this.Formula;
      foreach (string key in list)
      {
        string newValue;
        if (!dictionary.TryGetValue(key, out newValue))
          newValue = "0";
        formula = formula.Replace("<<Trait:" + key + ">>", newValue);
      }
      this.ValueToAdd = Utility.calculateValue(formula, (object) addedToThis).ToString() ?? "";
      base.ActionsOnAdd(addedToThis);
    }
  }
}
