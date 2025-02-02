
// Type: com.digitalarcsystems.Traveller.DataModel.TraitQualFilter




using Newtonsoft.Json;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class TraitQualFilter : CareerQualFilter
  {
    [JsonProperty]
    public Trait mustHave;

    [JsonConstructor]
    public TraitQualFilter()
    {
    }

    public TraitQualFilter(Trait required)
    {
      this.mustHave = required;
      this.Description = "Characters must be " + this.mustHave.Name;
    }

    public override bool passFilter(Character character)
    {
      EngineLog.Print("TraitQualFilter: passFilter() start.");
      EngineLog.Print("Character needs trait " + this.mustHave.Name);
      EngineLog.Print("Character has trait: " + character.hasTrait(this.mustHave).ToString());
      return character.hasTrait(this.mustHave);
    }
  }
}
