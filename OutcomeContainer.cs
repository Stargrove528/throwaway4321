
// Type: com.digitalarcsystems.Traveller.OutcomeContainer




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface OutcomeContainer
  {
    IList<Outcome> Outcomes { get; }
  }
}
