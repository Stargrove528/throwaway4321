
// Type: com.digitalarcsystems.Traveller.DataModel.NamedList`1




using System.Collections.Generic;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class NamedList<T> : List<T>
  {
    public NamedList(string name) => this.Name = name;

    public NamedList(string name, ICollection<T> collection)
      : base((IEnumerable<T>) collection)
    {
      this.Name = name;
    }

    public virtual string Name { get; private set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("[" + this.Name + "]\n");
      foreach (T obj in (List<T>) this)
        stringBuilder.Append(obj?.ToString() + "\n");
      return stringBuilder.ToString();
    }
  }
}
