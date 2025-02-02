
// Type: com.digitalarcsystems.Traveller.DataModel.EntityToMeet




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class EntityToMeet
  {
    public EntityToMeet.EntityType Type { get; set; }

    public string Notes { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public enum EntityType
    {
      CONTACT,
      ALLY,
      RIVAL,
      ENEMY,
    }
  }
}
