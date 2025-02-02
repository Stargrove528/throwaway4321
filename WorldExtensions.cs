
// Type: com.digitalarcsystems.Traveller.DataModel.WorldExtensions




using System;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public static class WorldExtensions
  {
    public static Guid GetSector(this World world)
    {
      return DataManager.Instance.Sectors.Where<Sector>((Func<Sector, bool>) (sector => sector.Worlds.Contains(world))).Select<Sector, Guid>((Func<Sector, Guid>) (sector => sector.Id)).FirstOrDefault<Guid>();
    }

    public static string GetSectorName(this World world)
    {
      Sector sector = DataManager.Instance.GetAsset<Sector>().FirstOrDefault<Sector>((Func<Sector, bool>) (x => x.Worlds.Contains(world)));
      return sector != null ? sector.Name : "";
    }

    public static string GetSubSectorName(this World world)
    {
      Sector sector = DataManager.Instance.GetAsset<Sector>().FirstOrDefault<Sector>((Func<Sector, bool>) (x => x.Worlds.Contains(world)));
      return sector != null ? sector.getSubsector(world.hexNumber) : "";
    }
  }
}
