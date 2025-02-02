
// Type: com.digitalarcsystems.Traveller.CategoryStringFilter




using com.digitalarcsystems.Traveller.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class CategoryStringFilter : ICareerFilter
  {
    [JsonProperty]
    private string _categoryTarget;
    [JsonProperty]
    private CategoryStringFilter.FilterBehavior _filterBehavior;
    [JsonProperty]
    public string drifterCareerName;

    [JsonConstructor]
    public CategoryStringFilter()
    {
    }

    public CategoryStringFilter(
      string stringToSearchFor,
      string drifterName,
      CategoryStringFilter.FilterBehavior filterBehavior)
      : this(stringToSearchFor, drifterName)
    {
      this._filterBehavior = filterBehavior;
    }

    public CategoryStringFilter(string stringToSearchFor, string drifterName)
      : this(stringToSearchFor)
    {
      this.drifterCareerName = drifterName;
    }

    public CategoryStringFilter(string stringToSearchFor)
      : this(stringToSearchFor, CategoryStringFilter.FilterBehavior.FILTER_OUT)
    {
    }

    public CategoryStringFilter(
      string stringToSearchFor,
      CategoryStringFilter.FilterBehavior filterBehavior)
    {
      this._categoryTarget = stringToSearchFor.ToLowerInvariant();
      this._filterBehavior = filterBehavior;
    }

    public bool filter(Career career)
    {
      bool flag = career.Category.ToLowerInvariant().Contains(this._categoryTarget);
      return this._filterBehavior == CategoryStringFilter.FilterBehavior.FILTER_OUT ? !flag : flag;
    }

    public Career GetDrifterEquivalentCareer(IList<Career> careers)
    {
      return careers == null ? (Career) null : careers.FirstOrDefault<Career>((Func<Career, bool>) (c => c.Name.Equals(this.drifterCareerName, StringComparison.InvariantCultureIgnoreCase)));
    }

    public enum FilterBehavior
    {
      FILTER_OUT,
      MUST_INCLUDE,
    }
  }
}
