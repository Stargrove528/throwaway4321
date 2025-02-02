
// Type: com.digitalarcsystems.Traveller.utility.DescribableContainer`1




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller.utility
{
  public class DescribableContainer<T> : Describable
  {
    public T Value { get; set; }

    public bool PrependValueDescription { get; set; }

    public DescribableContainer() => this.PrependValueDescription = true;

    public DescribableContainer(string Name, T Value)
      : this()
    {
      this.SetName(Name);
      this.Value = Value;
    }

    public DescribableContainer(string Name, string Description, T Value)
      : this(Name, Value)
    {
      this.SetDescription(Description);
    }

    public new virtual string Description
    {
      get
      {
        string description = base.Description;
        if ((object) this.Value is IDescribable && this.PrependValueDescription)
          description = ((IDescribable) (object) this.Value).Description + "\n\n" + base.Description;
        return description;
      }
    }
  }
}
