





using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class AllowEverythingCategoryFilter : ICareerFilter
  {
    [JsonConstructor]
    public AllowEverythingCategoryFilter()
    {
    }

    public bool filter(Career career) => true;

    public Career GetDrifterEquivalentCareer(IList<Career> careers)
    {
      return careers.FirstOrDefault<Career>((Func<Career, bool>) (c => c.Name.Equals("drifter", StringComparison.InvariantCultureIgnoreCase)));
    }
  }
}
