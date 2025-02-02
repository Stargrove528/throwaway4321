
// Type: com.digitalarcsystems.Traveller.ICareerFilter




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface ICareerFilter
  {
    bool filter(Career career);

    Career GetDrifterEquivalentCareer(IList<Career> careers);
  }
}
