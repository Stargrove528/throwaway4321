
// Type: com.digitalarcsystems.Traveller.ICareerSource




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface ICareerSource
  {
    IList<Career> Careers { get; }

    Career GetCareer(string careerName);

    IList<Career> GetCareersByCategory(string category);

    IList<PreCareer> PreCareers { get; }

    void SetCareerFilter(ICareerFilter filter);
  }
}
