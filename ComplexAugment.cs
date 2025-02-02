
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ComplexAugment




using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ComplexAugment : Augmentation
  {
    private readonly List<IAugmentation> _augments = new List<IAugmentation>();

    [JsonConstructor]
    public ComplexAugment(List<string> allowedUpgradeCategories)
      : base(allowedUpgradeCategories)
    {
    }

    public ComplexAugment()
    {
    }

    public ComplexAugment(IAugmentation copyMe)
      : base(copyMe)
    {
    }

    public void AddAgument(IAugmentation addMe) => this._augments.Add(addMe);

    public override void ActionsOnAdd(Character changeMe)
    {
      foreach (IAugmentation augment in this._augments)
      {
        augment.IsPartOfAnotherAugment = true;
        changeMe.Equip((IEquipment) augment);
      }
    }

    public override void ActionsOnRemoval(Character restoreMe)
    {
      foreach (IAugmentation augment in this._augments)
        restoreMe.UnEquip((IEquipment) augment);
    }
  }
}
