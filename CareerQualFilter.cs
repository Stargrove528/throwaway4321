
// Type: com.digitalarcsystems.Traveller.DataModel.CareerQualFilter




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class CareerQualFilter : IDescribable
  {
    public CareerQualFilter()
    {
      this.Name = string.Empty;
      this.Description = string.Empty;
    }

    public virtual bool passFilter(Character character) => false;

    public virtual string Description { get; set; }

    public virtual string Name { get; set; }
  }
}
