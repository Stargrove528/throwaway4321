
// Type: com.digitalarcsystems.Traveller.IDecisionMaker




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IDecisionMaker
  {
    IList<T> ResolveAllocation<T>(int quantity_to_allocate, IList<T> allocations) where T : IAllocatable;

    IList<T> Choose<T>(int numToChoose, IList<T> choices);

    T ChooseOne<T>(IList<T> choices);

    T ChooseOne<T>(string description, IList<T> choices);

    IList<T> Choose<T>(string description, int numToChoose, IList<T> choices);

    void setInfoForUI(CreationOperation currentOperation);

    void setQueryKey(ContextKeys key);

    void setResultKey(ContextKeys key);

    IDescribable provideFreeString(
      string description,
      string title = "",
      RequiredInformation requiredInfo = RequiredInformation.DESCRIPTION);

    void present(Presentation presentMeToUser);
  }
}
