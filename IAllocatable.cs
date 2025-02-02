
// Type: com.digitalarcsystems.Traveller.IAllocatable




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IAllocatable : IDescribable
  {
    int cost { get; set; }

    int amount_allocated { get; set; }

    int max_allocation { get; set; }
  }
}
