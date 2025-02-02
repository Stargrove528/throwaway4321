
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.StatAugmentation




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class StatAugmentation : Augmentation
  {
    public const int NoValue = -224;
    private int _previousStatValue = -224;

    public int MaxStatValue { get; set; } = -224;

    public bool SetStatToEqualAugmentationBonus { get; set; }

    public string Stat { get; set; }

    [JsonConstructor]
    public StatAugmentation(List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
    }

    public StatAugmentation()
    {
    }

    public StatAugmentation(IAugmentation copyMe)
      : base(copyMe)
    {
    }

    public override void ActionsOnAdd(Character changeMe)
    {
      this.ModifyStat(changeMe, this.AugmentationBonus);
    }

    public override void ActionsOnRemoval(Character restoreMe)
    {
      this.ModifyStat(restoreMe, -1 * this.AugmentationBonus);
    }

    private void ModifyStat(Character modifyMe, int value)
    {
      com.digitalarcsystems.Traveller.DataModel.Attribute attribute = modifyMe.getAttribute(this.Stat);
      if (this.SetStatToEqualAugmentationBonus)
      {
        if (this._previousStatValue == -224)
        {
          this._previousStatValue = attribute.Value;
          attribute.Value = this.AugmentationBonus;
          attribute.UninjuredValue = this.AugmentationBonus;
        }
        else
        {
          attribute.UninjuredValue = this._previousStatValue;
          this._previousStatValue = -224;
        }
      }
      else
      {
        string[] strArray1 = new string[6]
        {
          "BEFORE AugmentValue: ",
          value.ToString(),
          "\tStatValue: ",
          attribute.Value.ToString(),
          "\tUninjured: ",
          null
        };
        int uninjuredValue = attribute.UninjuredValue;
        strArray1[5] = uninjuredValue.ToString();
        EngineLog.Print(string.Concat(strArray1));
        string[] strArray2 = new string[6]
        {
          "BEFORE AugmentValue: ",
          value.ToString(),
          "\tStatValue: ",
          null,
          null,
          null
        };
        uninjuredValue = attribute.Value;
        strArray2[3] = uninjuredValue.ToString();
        strArray2[4] = "\tUninjured: ";
        uninjuredValue = attribute.UninjuredValue;
        strArray2[5] = uninjuredValue.ToString();
        Console.WriteLine(string.Concat(strArray2));
        attribute.Value += value;
        attribute.UninjuredValue += value;
        string[] strArray3 = new string[6]
        {
          "AFTER AugmentValue: ",
          value.ToString(),
          "\tStatValue: ",
          null,
          null,
          null
        };
        uninjuredValue = attribute.Value;
        strArray3[3] = uninjuredValue.ToString();
        strArray3[4] = "\tUninjured: ";
        uninjuredValue = attribute.UninjuredValue;
        strArray3[5] = uninjuredValue.ToString();
        EngineLog.Print(string.Concat(strArray3));
        string[] strArray4 = new string[6]
        {
          "AFTER AugmentValue: ",
          value.ToString(),
          "\tStatValue: ",
          null,
          null,
          null
        };
        uninjuredValue = attribute.Value;
        strArray4[3] = uninjuredValue.ToString();
        strArray4[4] = "\tUninjured: ";
        uninjuredValue = attribute.UninjuredValue;
        strArray4[5] = uninjuredValue.ToString();
        Console.WriteLine(string.Concat(strArray4));
      }
    }
  }
}
