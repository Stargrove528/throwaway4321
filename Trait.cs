
// Type: com.digitalarcsystems.Traveller.DataModel.Trait




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Trait : IDescribable
  {
    [JsonIgnore]
    public static Trait PSIONIC = new Trait("Psionic", "The ability to effect yourself and the world with your mind", "Psi", 6, -1);
    public static Trait ZHODANI_PSIONIC = new Trait("Zhodani Psionic", "The ability to effect yourself and the world with your mind", "Psi", 6, -1);

    public string Name { get; set; }

    public string Description { get; set; }

    public string associatedAttribute { get; set; }

    public int ordinal { get; set; }

    public int attribute_modifier_per_term_not_possessed { get; set; }

    public Trait()
    {
    }

    public Trait(
      string name,
      string description,
      string associatedAttribute,
      int ordinal,
      int attribute_modifier_per_term_not_possessed)
    {
      this.Name = name;
      this.Description = description;
      this.associatedAttribute = associatedAttribute;
      this.ordinal = ordinal;
      this.attribute_modifier_per_term_not_possessed = attribute_modifier_per_term_not_possessed;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Trait)
      {
        Trait trait = (Trait) obj;
        if (trait.Name.Equals(this.Name, StringComparison.InvariantCultureIgnoreCase) && this.associatedAttribute.Equals(trait.associatedAttribute, StringComparison.InvariantCultureIgnoreCase))
          flag = true;
      }
      return flag;
    }
  }
}
