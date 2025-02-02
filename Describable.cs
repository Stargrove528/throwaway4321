
// Type: com.digitalarcsystems.Traveller.DataModel.Describable




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Describable : IDescribable
  {
    public string Description { get; set; }

    public string Name { get; set; }

    public Describable()
    {
    }

    public Describable(string withName, string withDescription)
    {
      this.Description = withDescription;
      this.Name = withName;
    }

    public void SetName(string newName) => this.Name = newName;

    public void SetDescription(string newDescription) => this.Description = newDescription;
  }
}
