
// Type: com.digitalarcsystems.Traveller.DataModel.Configuration




using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Configuration : INonLicensedAsset, IAssetBase, IAsset, IDescribable
  {
    [JsonIgnore]
    public const float NOTAPPLICABLE = -3.40282347E+38f;
    [JsonIgnore]
    public const int INT_NOTAPPLICABLE = -2147483648;
    [JsonIgnore]
    public const float TRUE = 1f;
    [JsonIgnore]
    public const float FALSE = -3.40282347E+38f;
    public Configuration.ConfigurationType configurationType = Configuration.ConfigurationType.BOOLEAN;
    public List<IDescribable> choices;
    [JsonProperty]
    protected bool bool_value = false;
    [JsonProperty]
    protected float max_value = float.MinValue;
    [JsonProperty]
    protected float min_value = float.MinValue;
    [JsonProperty]
    protected float num_of_choices_required;
    [JsonProperty]
    protected float float_value = float.MinValue;
    [JsonProperty]
    protected int int_value = int.MinValue;
    [JsonProperty]
    protected string string_value = (string) null;
    [JsonProperty]
    protected List<IDescribable> selectedChoices = new List<IDescribable>();
    public string _defaultFileName = "";

    public Dictionary<Guid, AssetMetadata> ChildAssets { get; set; }

    public List<AssetTag> Tags { get; set; }

    [JsonProperty]
    public bool Enable { get; set; }

    public NamedList<Configuration> Children { get; set; }

    public string ConfiguratorName { get; set; }

    public string DefaultFileName => this._defaultFileName;

    public Guid Id { get; set; }

    public int CreatingUser { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    [JsonIgnore]
    public float MaxValue => this.max_value;

    [JsonIgnore]
    public float MinValue => this.min_value;

    public bool asBoolean() => this.bool_value;

    public float asFloat() => this.float_value;

    public int asInt() => this.int_value;

    public IDescribable asSingleSelection()
    {
      IDescribable describable = (IDescribable) null;
      if (this.selectedChoices != null && this.selectedChoices.Count > 0)
        describable = this.selectedChoices[0];
      return describable;
    }

    public List<IDescribable> asMultipleSelections() => this.selectedChoices;

    public Configuration setValue(bool boolValue)
    {
      this.bool_value = boolValue;
      if (this.configurationType == Configuration.ConfigurationType.BOOLEAN)
        this.Enable = true;
      return this;
    }

    public Configuration setValue(float floatValue)
    {
      this.float_value = floatValue;
      return this;
    }

    public Configuration setValue(int intValue)
    {
      this.int_value = intValue;
      return this;
    }

    public Configuration setValue(string stringValue)
    {
      this.string_value = stringValue;
      return this;
    }

    public Configuration setValue(List<IDescribable> choices)
    {
      this.selectedChoices.Clear();
      this.selectedChoices.AddRange((IEnumerable<IDescribable>) choices);
      return this;
    }

    public Configuration setValue(IDescribable choice)
    {
      this.selectedChoices.Clear();
      this.selectedChoices.Add(choice);
      return this;
    }

    public Configuration()
    {
      this.Tags = new List<AssetTag>();
      this.ChildAssets = new Dictionary<Guid, AssetMetadata>();
    }

    [JsonConstructor]
    public Configuration(
      bool enabled,
      bool bool_value,
      float max_value,
      float min_value,
      float num_of_choices_required,
      float float_value,
      int int_value,
      string string_value,
      List<IDescribable> selectedChoices)
    {
      this.Enable = enabled;
      this.setValue(bool_value);
      this.max_value = max_value;
      this.min_value = min_value;
      this.num_of_choices_required = num_of_choices_required;
      this.float_value = float_value;
      this.int_value = int_value;
      this.string_value = string_value;
      this.selectedChoices = selectedChoices;
    }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = false;
      if (obj is Configuration && ((Configuration) obj).ConfiguratorName.CompareTo(this.ConfiguratorName) == 0)
        flag = true;
      return flag;
    }

    public override int GetHashCode() => this.ConfiguratorName.GetHashCode();

    public enum ConfigurationType
    {
      BOOLEAN,
      INTEGER,
      FLOAT,
      CHOICES,
      STRING,
    }
  }
}
