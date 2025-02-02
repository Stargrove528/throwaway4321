
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.CharacterPropertyAugmentation




using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class CharacterPropertyAugmentation : Augmentation
  {
    private PropertyInfo _prop;
    private object _previousValue;

    public string PropertyName { get; set; }

    public object NewValue { get; set; }

    [JsonConstructor]
    public CharacterPropertyAugmentation(List<string> allowedUpgradeTypes)
      : base(allowedUpgradeTypes)
    {
    }

    public CharacterPropertyAugmentation()
    {
    }

    public CharacterPropertyAugmentation(IAugmentation copyMe)
      : base(copyMe)
    {
    }

    public override int AugmentationBonus
    {
      get
      {
        int result;
        return int.TryParse(this.NewValue?.ToString() ?? "", out result) ? result : base.AugmentationBonus;
      }
      set
      {
        if (Utility.IsNumeric(this.NewValue))
          this.NewValue = (object) value;
        else
          base.AugmentationBonus = value;
      }
    }

    public override void ActionsOnAdd(Character changeMe)
    {
      this._prop = typeof (Character).GetProperty(this.PropertyName);
      this._previousValue = this._prop.GetValue((object) changeMe, (object[]) null);
      this._prop.SetValue((object) changeMe, this.NewValue, (object[]) null);
    }

    public override void ActionsOnRemoval(Character restoreMe)
    {
      this._prop.SetValue((object) restoreMe, this._previousValue, (object[]) null);
      this._previousValue = (object) null;
    }
  }
}
