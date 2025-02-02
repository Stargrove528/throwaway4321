
// Type: com.digitalarcsystems.Traveller.DataModel.IAddable`1




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface IAddable<in T> : IDescribable
  {
    int Level { get; set; }

    void Add(T addMe);
  }
}
