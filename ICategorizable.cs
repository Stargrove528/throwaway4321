
// Type: com.digitalarcsystems.Traveller.DataModel.ICategorizable




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface ICategorizable : IDescribable
  {
    List<string> Categories { get; }
  }
}
