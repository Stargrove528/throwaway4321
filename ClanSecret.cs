
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.ClanSecret




using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class ClanSecret : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
  {
    public ClanSecret()
    {
      this.Name = "Clan Secret";
      this.Description = "You uncover an embarassing secret related to your clan or family.  Either trade it for 1D Clan shares, or you can keep it in reserve.  Whenever you use this secret, gain a clan elder as an Enemy.";
      this.InstanceID = Guid.NewGuid();
      this.Id = new Guid("b7372242-3419-4cfa-b85f-13d5698a362d");
    }
  }
}
