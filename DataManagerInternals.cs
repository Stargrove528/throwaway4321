
// Type: com.digitalarcsystems.Traveller.DataManagerInternals




using com.digitalarcsystems.Traveller.DataModel;
using System.IO;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class DataManagerInternals : IDataManagerInternals
  {
    private readonly DataManager _instance;

    internal DataManagerInternals(DataManager instance) => this._instance = instance;

    public void ImportAssetsFromDirectory(
      string directoryPath,
      string searchPattern = "*.*",
      SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
      this._instance.ImportAssetsFromDirectory(directoryPath, searchPattern, searchOption);
    }

    public void ImportAssetFromFile(string fileName, bool skipUnknownFileTypes = false)
    {
      this._instance.ImportAssetFromFile(fileName, skipUnknownFileTypes);
    }

    public void DeleteLocal(IAssetBase asset, bool save = false)
    {
      this._instance.DeleteLocal(asset, save);
    }

    public void AddOrUpdateAsset(IAssetBase asset) => this._instance.AddOrUpdateAsset(asset);
  }
}
