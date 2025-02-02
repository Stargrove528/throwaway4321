
// Type: com.digitalarcsystems.Traveller.DataModel.ICustomDescribable




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface ICustomDescribable : IDescribable
  {
    string OriginalName { get; }

    string OriginalDescription { get; }
  }
}
