
// Type: com.digitalarcsystems.Traveller.Context




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class Context : Dictionary<string, object>
  {
    public object GetNullableContextObject(ContextKeys withKey)
    {
      if (this.ContainsKey(withKey.ToString()))
        return this[withKey.ToString()];
      EngineLog.Warning("null value in context for key: " + withKey.ToString());
      return (object) null;
    }

    public void AddWithKey(ContextKeys key, object data) => this.Add(key.ToString(), data);

    public void Add(ContextKeys key, object data) => this.AddWithKey(key, data);

    public object this[ContextKeys key]
    {
      get => this[key.ToString()];
      set => this.Add(key, value);
    }
  }
}
