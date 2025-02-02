
// Type: com.digitalarcsystems.Traveller.IPeopleSource




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IPeopleSource
  {
    Contact EntityToMeet { get; }

    IList<Contact> getEntityToMeet(int num, string description = "");
  }
}
