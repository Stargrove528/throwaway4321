
// Type: com.digitalarcsystems.Traveller.IDataManagerInternals




using com.digitalarcsystems.Traveller.DataModel;
using System.IO;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface IDataManagerInternals
  {
    void ImportAssetsFromDirectory(
      string directoryPath,
      string searchPattern = "*.*",
      SearchOption searchOption = SearchOption.TopDirectoryOnly);

    void ImportAssetFromFile(string fileName, bool skipUnknownFileTypes = false);

    void DeleteLocal(IAssetBase asset, bool save = false);

    void AddOrUpdateAsset(IAssetBase asset);
  }
}
