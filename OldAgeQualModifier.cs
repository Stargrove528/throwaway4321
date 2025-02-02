
// Type: com.digitalarcsystems.Traveller.DataModel.OldAgeQualModifier




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class OldAgeQualModifier : CareerQualModifier
  {
    [JsonConstructor]
    public OldAgeQualModifier()
    {
    }

    public OldAgeQualModifier(int age_limit) => this.age_limit = age_limit;

    public OldAgeQualModifier(int age_limit, int modifier)
      : this(age_limit)
    {
      this.modifier = modifier;
    }

    public override int getQualModifier(Character character)
    {
      int qualModifier = 0;
      if (character.Age >= this.age_limit)
        qualModifier += this.modifier;
      if (qualModifier < -1000)
        Console.WriteLine("Adjusted modifier is wayy off base");
      return qualModifier;
    }
  }
}
