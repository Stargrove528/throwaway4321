
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IAmmoAction




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IAmmoAction
  {
    string AdditionalInfo { get; set; }

    void ActionsOnAdd(IMultishotRangedWeapon addedToThis);

    void ActionsOnRemoval(IMultishotRangedWeapon removedFromThis);
  }
}
