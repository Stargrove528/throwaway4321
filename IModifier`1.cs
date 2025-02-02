
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.IModifier`1




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public interface IModifier<T>
  {
    void ActionsOnAdd(T addedToThis);

    void ActionsOnRemoval(T removedFromThis);
  }
}
