
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.PriceCalculatingEquipmentOption




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class PriceCalculatingEquipmentOption : AbstractEquipmentOption
  {
    [JsonProperty]
    private readonly List<string> _tokens;
    [JsonIgnore]
    public static readonly string[] Delimiters = new string[2]
    {
      "<<",
      ">>"
    };
    [JsonProperty]
    private readonly string _costFormula;

    [JsonConstructor]
    public PriceCalculatingEquipmentOption()
    {
    }

    public PriceCalculatingEquipmentOption(string costFormula, IEquipment copyMe)
      : base(copyMe)
    {
      this._costFormula = costFormula;
      this._tokens = ((IEnumerable<string>) costFormula.Split(new string[1]
      {
        "<<"
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (s => s.Contains(">>"))).Select<string, string>((Func<string, string>) (s => s.Split(new string[1]
      {
        ">>"
      }, StringSplitOptions.RemoveEmptyEntries)[0])).ToList<string>();
      if (this._tokens == null || !this._tokens.Any<string>())
        throw new ArgumentException("costFormula contained no tokens to parse, or was not parsable.  Please consider using a FixedPriceEquipmentOption.");
    }

    public PriceCalculatingEquipmentOption(string costFormula, AbstractEquipmentOption copyMe)
      : base(copyMe)
    {
      this._costFormula = costFormula;
      this._tokens = ((IEnumerable<string>) costFormula.Split(new string[1]
      {
        "<<"
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (s => s.Contains(">>"))).Select<string, string>((Func<string, string>) (s => s.Split(new string[1]
      {
        ">>"
      }, StringSplitOptions.RemoveEmptyEntries)[0])).ToList<string>();
      if (this._tokens == null || !this._tokens.Any<string>())
        throw new ArgumentException("costFormula contained no tokens to parse, or was not parsable.  Please consider using a FixedPriceEquipmentOption.");
    }

    public PriceCalculatingEquipmentOption(string costFormula)
    {
      this._costFormula = costFormula;
      this._tokens = ((IEnumerable<string>) costFormula.Split(new string[1]
      {
        "<<"
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (s => s.Contains(">>"))).Select<string, string>((Func<string, string>) (s => s.Split(new string[1]
      {
        ">>"
      }, StringSplitOptions.RemoveEmptyEntries)[0])).ToList<string>();
      if (this._tokens == null || !this._tokens.Any<string>())
        throw new ArgumentException("costFormula contained no tokens to parse, or was not parsable.  Please consider using a FixedPriceEquipmentOption.");
    }

    public override int CalculatePrice(IEquipment equipmentToBeAddedTo)
    {
      return Utility.calculateValue(this._costFormula, (object) equipmentToBeAddedTo);
    }
  }
}
