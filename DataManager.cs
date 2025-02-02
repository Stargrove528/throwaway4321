
// Type: com.digitalarcsystems.Traveller.DataManager




using com.digitalarcsystems.Traveller.DataModel;
using com.digitalarcsystems.Traveller.DataModel.Equipment;
using com.digitalarcsystems.Traveller.utility;
using Newtonsoft.Json;
using RPGSuiteCloud;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using TCG_CS_Engine.Annotations;
using TCG_CS_Engine.src.com.digitalarcsystems.Traveller.DataManagerHelpers;
using TCG_CS_Engine.src.com.digitalarcsystems.Traveller.utility;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public sealed class DataManager : IDataManager
  {
    private static CacheLocationManager clm;
    public static string BASIC_ASSETS_JSON = "TCG_CS_Engine.resources.BasicAssets.json";
    private readonly bool _encrypt = false;
    private static string _customPath = "";
    public static int UserID;
    private static readonly object SyncRoot = new object();
    private static volatile DataManager _dataManager;
    private readonly string _lockfile;
    private readonly QueuedWriter _writer;
    private Dictionary<string, S3AssetMetadata> s3Assets = new Dictionary<string, S3AssetMetadata>();
    [UsedImplicitly]
    private static bool _isAssetAuthoringDataManager;
    private Api _siteAPI;
    private int _numTimesInitializedCalled = 0;
    private int _ulacNumtries = 0;
    private volatile object _onDownloadCompleteLock = new object();
    public static bool IsUploading;
    public static bool IsDownloadingNonLicensed;
    public static bool IsDownloadingLicensed;
    private volatile object _onUploadCompleteLock = new object();
    private volatile object _onErrorsLock = new object();
    private volatile object _onReadyLock = new object();
    private volatile object _onUploadLock = new object();
    private volatile object _onDownloadProgressLock = new object();
    private int _licensedAssetDownloadCount;
    private bool _licensedStart;
    private int _nonLicensedUploadCount;
    private int _nonLicensedDownloadCount;
    private bool _nonlicensedDownloadStart;
    private bool _nonlicensedUploadStart;
    private volatile object _cleanupLock = new object();
    private volatile object _syncLock = new object();
    public static bool IsLicensedClean;
    public static bool IsNonLicensedClean;

    public static bool DecryptConfigFile { get; set; }

    public static string CustomPath
    {
      get => DataManager._customPath;
      set
      {
        if (DataManager._dataManager != null && value != DataManager._customPath)
          throw new InvalidOperationException("CustomPath must be set BEFORE DataManager.Instance is referenced");
        DataManager._customPath = value;
      }
    }

    internal static DataManager Instance
    {
      get
      {
        if (DataManager._dataManager == null)
        {
          lock (DataManager.SyncRoot)
          {
            string str;
            switch (Environment.OSVersion.Platform)
            {
              case PlatformID.Unix:
                str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local", "share", "DigitalArcSystems");
                break;
              case PlatformID.MacOSX:
                str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support", "<YourAppName>");
                break;
              default:
                str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "DigitalArcSystems");
                break;
            }
            if (!string.IsNullOrEmpty(DataManager.CustomPath))
              str = Path.Combine(DataManager.CustomPath, "DigitalArcSystems");
            if (!Directory.Exists(str))
              Directory.CreateDirectory(str);
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
              str.Replace('.', '\\');
            else
              str.Replace('.', '/');
            DataManager._dataManager = new DataManager(str);
          }
        }
        return DataManager._dataManager;
      }
    }

    public IList<Career> Careers => (IList<Career>) this.GetAsset<Career>().ToList<Career>();

    public IList<Character> Characters
    {
      get => (IList<Character>) this.GetAsset<Character>().ToList<Character>();
    }

    public string DM_CacheDirectory { get; }

    public IList<Contact> EntitiesToMeet
    {
      get => (IList<Contact>) this.GetAsset<Contact>().ToList<Contact>();
    }

    public IList<IEquipment> Equipment
    {
      get => (IList<IEquipment>) this.GetAsset<IEquipment>().ToList<IEquipment>();
    }

    public IList<GenerationState> GenerationStates
    {
      get => (IList<GenerationState>) this.GetAsset<GenerationState>().ToList<GenerationState>();
    }

    public IList<com.digitalarcsystems.Traveller.DataModel.AssetMetadata> LicensedAssetMetadataList
    {
      get
      {
        return (IList<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>) this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (c => c.TheAsset is ILicensedAsset && !(c.TheAsset is INonLicensedAsset))).ToList<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>();
      }
    }

    public IList<com.digitalarcsystems.Traveller.DataModel.AssetMetadata> NonLicensedAssetMetadataList
    {
      get
      {
        return (IList<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>) this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (c => c.TheAsset is INonLicensedAsset && !(c.TheAsset is ILicensedAsset))).ToList<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>();
      }
    }

    public com.digitalarcsystems.Traveller.DataModel.AssetMetadata AssetMetadata(Guid id)
    {
      com.digitalarcsystems.Traveller.DataModel.AssetMetadata assetMetadata = (com.digitalarcsystems.Traveller.DataModel.AssetMetadata) null;
      this.Configuration.Cache.TryGetValue(id, out assetMetadata);
      return assetMetadata;
    }

    public IList<LicensedBinaryAsset> LicensedBinaryAssets
    {
      get
      {
        return (IList<LicensedBinaryAsset>) this.GetAsset<LicensedBinaryAsset>().ToList<LicensedBinaryAsset>();
      }
    }

    public DataManager.LogonStatus OnlineStatus { get; private set; }

    public IList<Race> Races => (IList<Race>) this.GetAsset<Race>().ToList<Race>();

    public IList<Sector> Sectors => (IList<Sector>) this.GetAsset<Sector>().ToList<Sector>();

    public IList<ISkill> Skills
    {
      get
      {
        return (IList<ISkill>) this.GetAsset<ISkill>().OrderBy<ISkill, string>((Func<ISkill, string>) (s => s.Name)).ToList<ISkill>();
      }
    }

    public IList<Talent> Talents
    {
      get
      {
        return (IList<Talent>) this.GetAsset<Talent>().OrderBy<Talent, string>((Func<Talent, string>) (s => s.Name)).ToList<Talent>();
      }
    }

    public IList<World> Worlds
    {
      get
      {
        return (IList<World>) this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (w => w.TheAsset is Sector)).Select<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, List<World>>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, List<World>>) (w => ((Sector) w.TheAsset).Worlds)).ToList<List<World>>().SelectMany<List<World>, World>((Func<List<World>, IEnumerable<World>>) (x => (IEnumerable<World>) x)).ToList<World>();
      }
    }

    public static DataManager AssetAuthoringDataManager(
      string path,
      Action<IDataManagerInternals> provideSecretInterface = null)
    {
      if (DataManager._dataManager != null)
        throw new ApplicationException("Warning, data manager initialized twice because you called the wrong accessor; this probably isn't what you meant to do. ");
      DataManager._isAssetAuthoringDataManager = true;
      try
      {
        DataManager._dataManager = new DataManager(path);
      }
      catch (Exception ex)
      {
        Console.WriteLine("ERROR BRINGING UP ASSET AUTHORING DATA MANAGER: " + path + "\n\n" + ex.Message + "\n\n\n" + ex.StackTrace);
        throw ex;
      }
      if (provideSecretInterface != null)
        provideSecretInterface((IDataManagerInternals) new DataManagerInternals(DataManager._dataManager));
      return DataManager._dataManager;
    }

    public bool AddIfNewLicensedBinaryAsset(LicensedBinaryAsset alreadyExist)
    {
      LicensedBinaryAsset instance = this.LicensedBinaryAssets.FirstOrDefault<LicensedBinaryAsset>((Func<LicensedBinaryAsset, bool>) (a => a.Id == alreadyExist.Id));
      if (instance != null && instance.SHA1<LicensedBinaryAsset>().Equals(alreadyExist.SHA1<LicensedBinaryAsset>()))
        return false;
      if (alreadyExist.Name == null || alreadyExist.Name.Length == 0)
        alreadyExist.Name = Path.GetFileNameWithoutExtension(alreadyExist.ClientFilePath);
      this.AssetSerializerHandler((object) alreadyExist);
      return true;
    }

    public bool AddIfNewLicensedBinaryAsset(string path, Guid id, out LicensedBinaryAsset asset)
    {
      LicensedBinaryAsset licensedBinaryAsset = this.LicensedBinaryAssets.FirstOrDefault<LicensedBinaryAsset>((Func<LicensedBinaryAsset, bool>) (a => a.SourceFilename == path));
      if (licensedBinaryAsset != null)
      {
        asset = licensedBinaryAsset;
        return false;
      }
      asset = LicensedBinaryAsset.CreateFrom(path, id);
      asset.Name = Path.GetFileNameWithoutExtension(path);
      this.AssetSerializerHandler((object) asset, path);
      return true;
    }

    public void Delete(IEnumerable<INonLicensedAsset> assets)
    {
      foreach (INonLicensedAsset serializableAsset in assets.ToList<INonLicensedAsset>())
        this.Delete(serializableAsset, false);
      this.SaveConfiguration();
    }

    public void Delete(INonLicensedAsset serializableAsset, bool saveAfter = true)
    {
      if (serializableAsset == null)
      {
        EngineLog.Warning("Attempting to delete a null asset? Ignoring");
      }
      else
      {
        try
        {
          this.DeleteLocal((IAssetBase) serializableAsset, saveAfter);
          if (this.OnlineStatus != DataManager.LogonStatus.Authenticated)
            return;
          this._siteAPI.DeleteUserAsset(serializableAsset.Id, (Action<Guid, bool>) ((guid, b) =>
          {
            if (b)
              return;
            this.Log("Could not delete " + serializableAsset.DefaultFileName + " remotely.");
          }));
        }
        catch (Exception ex)
        {
          EngineLog.Error("Exception trying to delete remote asset! " + ex.Message);
        }
      }
    }

    private void DownloadLicensed(
      S3AssetMetadata s3AssetMetadata,
      Action<S3AssetMetadata, IAsset> callback)
    {
      try
      {
        this._siteAPI.Download(s3AssetMetadata.DownloadURL, (Action<byte[]>) (result =>
        {
          if (result != null)
          {
            try
            {
              byte[] numArray = result.DecryptCloudAsset();
              if (!LicensedBinaryAsset.IsValid(numArray))
              {
                callback(s3AssetMetadata, Encoding.UTF8.GetString(numArray.Decompress()).AsDeserialized<IAsset>());
              }
              else
              {
                LicensedBinaryAsset.CreateFrom(numArray);
                callback(s3AssetMetadata, (IAsset) LicensedBinaryAsset.LoadFrom(numArray));
              }
            }
            catch (CryptographicException ex)
            {
              callback(s3AssetMetadata, (IAsset) null);
              this.OnErrors(new DataManagerErrorEventArgs(string.Format("A cryptographic exception occurred for asset {0}: {1}", (object) s3AssetMetadata.AssetGuid, (object) ex.Message)));
            }
          }
          else
          {
            this.Log("Download failed");
            this.OnErrors(new DataManagerErrorEventArgs("An error occured while downloading a licensed asset"));
            callback((S3AssetMetadata) null, (IAsset) null);
          }
        }));
      }
      catch (APIException ex)
      {
        this.Log("caught exception downloading licensed asset:" + ex.Message);
        callback((S3AssetMetadata) null, (IAsset) null);
      }
    }

    public void DownloadNonlicensed(string url, Action<IAsset> callback)
    {
      if (!string.IsNullOrEmpty(url))
        this._siteAPI.Download(url, (Action<byte[]>) (result =>
        {
          if (result != null)
          {
            callback(Encoding.UTF8.GetString(result.Decompress()).AsDeserialized<IAsset>());
          }
          else
          {
            this.Log("Download failed.");
            this.OnErrors(new DataManagerErrorEventArgs("An error occurred while downloading a nonlicensed asset."));
            callback((IAsset) null);
          }
        }));
      else
        this.Log("Null URL was passed for nonlicensed download.");
    }

    public void Export(object saveMe, string pathAndName, bool saveInBackground = true)
    {
      Stopwatch sw = Stopwatch.StartNew();
      if (!saveInBackground)
      {
        string str = saveMe.AsSerialized();
        sw.Stop();
        this.Log("Serialization of {0} took {1}ms", (object) saveMe.GetType().Name, (object) sw.ElapsedMilliseconds);
        bool flag = !(saveMe is INonLicensedAsset);
        if (!string.IsNullOrEmpty(str))
        {
          if (saveInBackground)
            this._writer.Queue(pathAndName, str, flag && this._encrypt);
          else
            QueuedWriter.Write(pathAndName, str, this._encrypt & flag);
        }
        else
          this.Log("file " + pathAndName + " was not queued for save because it was not correctly serialized");
      }
      else
        saveMe.AsSerializedAsync((Action<string>) (data =>
        {
          sw.Stop();
          this.Log("Serialization of {0} took {1}ms", (object) saveMe.GetType().Name, (object) sw.ElapsedMilliseconds);
          bool flag = !(saveMe is INonLicensedAsset);
          if (!string.IsNullOrEmpty(data))
          {
            if (saveInBackground)
              this._writer.Queue(pathAndName, data, flag && this._encrypt);
            else
              QueuedWriter.Write(pathAndName, data, this._encrypt & flag);
          }
          else
          {
            this.Log("file " + pathAndName + " was not queued for save because it was not correctly serialized");
            Console.WriteLine("file " + pathAndName + " was not queued for save because it was not correctly serialized");
          }
        }));
    }

    public IEnumerable<com.digitalarcsystems.Traveller.DataModel.AssetMetadata> FindLicensedAssets(
      IEnumerable<Guid> guidsToMatch)
    {
      return this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (metadata => metadata.TheAsset is ILicensedAsset)).Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (metadata => guidsToMatch.Contains<Guid>(metadata.TheAsset.Id)));
    }

    public IEnumerable<T> GetAsset<T>(string name, bool isClone = false) where T : IAsset
    {
      name = name.Trim().ToLower();
      IEnumerable<T> list = this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (a => ((IDescribable) a.TheAsset).Name != null && ((IDescribable) a.TheAsset).Name.Trim().ToLower().Contains(name))).Select<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, IAssetBase>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, IAssetBase>) (a => a.TheAsset)).OfType<T>();
      if (isClone)
        list = (IEnumerable<T>) this.InitializeInstanceIDForEquipment<T>((IEnumerable<T>) list.Clone<T>().ToList<T>());
      return list;
    }

    public IEnumerable<T> GetAsset<T>(List<string> names, bool isClone = false) where T : IAsset
    {
      List<T> asset = new List<T>();
      foreach (string name in names)
        asset.AddRange(this.GetAsset<T>(name, isClone));
      return (IEnumerable<T>) asset;
    }

    public IEnumerable<T> GetAsset<T>(IEnumerable<Guid> guids, bool isClone = false) where T : IAsset
    {
      return (IEnumerable<T>) guids.Select<Guid, T>((Func<Guid, T>) (x => this.GetAsset<T>(x, isClone))).ToList<T>();
    }

    public T GetAsset<T>(Guid targetGuid, bool isClone = false) where T : IAsset
    {
      T asset = default (T);
      com.digitalarcsystems.Traveller.DataModel.AssetMetadata assetMetadata;
      if (this.Configuration.Cache.TryGetValue(targetGuid, out assetMetadata))
        asset = (T) assetMetadata.TheAsset;
      if (isClone)
      {
        asset = asset.Clone<T>();
        this.InitializeInstanceIDForEquipment<T>(asset);
      }
      return asset;
    }

    public IEnumerable<T> GetAsset<T>(bool isClone = false, params AssetTag[] tags) where T : IAsset
    {
      IList<T> asset = (IList<T>) this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (a => a != null && a.TheAsset != null && a.TheAsset is T)).Select<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, IAssetBase>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, IAssetBase>) (a => a.TheAsset)).ToList<IAssetBase>().Where<IAssetBase>((Func<IAssetBase, bool>) (a => !((T) a).Tags.Except<AssetTag>((IEnumerable<AssetTag>) ((IEnumerable<AssetTag>) tags).ToList<AssetTag>()).Any<AssetTag>())).Select<IAssetBase, T>((Func<IAssetBase, T>) (val => (T) val)).ToList<T>();
      if (!asset.Any<T>())
        return (IEnumerable<T>) null;
      if (isClone)
        asset = this.InitializeInstanceIDForEquipment<T>((IEnumerable<T>) Extensions.Clone<IList<T>>(asset).ToList<T>());
      return (IEnumerable<T>) asset;
    }

    public IEnumerable<T> GetAsset<T>(Func<T, bool> func = null, bool isClone = false) where T : IAsset
    {
      IEnumerable<T> initializeUs = this.Configuration.Cache.Values.Where<com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, bool>) (a =>
      {
        if (a == null || a.TheAsset == null || !(a.TheAsset is T))
          return false;
        return func == null || func((T) a.TheAsset);
      })).Select<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, T>((Func<com.digitalarcsystems.Traveller.DataModel.AssetMetadata, T>) (a => !isClone ? (T) a.TheAsset : ((T) a.TheAsset).Clone<T>()));
      return isClone ? (IEnumerable<T>) this.InitializeInstanceIDForEquipment<T>(initializeUs) : initializeUs;
    }

    private void InitializeInstanceIDForEquipment<T>(T initializeMe) where T : IAsset
    {
      if (!((object) initializeMe is IEquipment))
        return;
      ((IEquipment) (object) initializeMe).InstanceID = Guid.NewGuid();
    }

    private IList<T> InitializeInstanceIDForEquipment<T>(IEnumerable<T> initializeUs) where T : IAsset
    {
      IList<T> list = (IList<T>) initializeUs.ToList<T>();
      for (int index = 0; index < list.Count<T>(); ++index)
      {
        if ((object) list[index] is IEquipment)
          ((IEquipment) (object) list[index]).InstanceID = Guid.NewGuid();
      }
      return list;
    }

    public Dictionary<T, DateTime> GetAssetsAndModifiedTimes<T>(bool isClone = false) where T : IAsset, INonLicensedAsset
    {
      return this.GetAsset<T>((Func<T, bool>) (x => true), isClone).ToDictionary<T, T, DateTime>((Func<T, T>) (k => k), (Func<T, DateTime>) (v => this.Configuration.Cache[v.Id].LastModified));
    }

    public T GetRandom<T>(Func<T, bool> func = null, bool isClone = false) where T : IAsset
    {
      Random random = new Random();
      List<T> list = this.GetAsset<T>(func).ToList<T>();
      int index = random.Next(list.Count);
      return list[index];
    }

    public void LoadLicensedBinaryAssets(string path)
    {
      foreach (object obj in ((IEnumerable<string>) Directory.GetFiles(path, ".trlic", SearchOption.TopDirectoryOnly)).Select<string, LicensedBinaryAsset>(new Func<string, LicensedBinaryAsset>(LicensedBinaryAsset.LoadFrom)))
        this.AssetSerializerHandler(obj);
    }

    public static void LogIn(
      ClientSettings settings,
      Action<DataManager, UserAccessRecord, string> loginCallback,
      bool forceOfflineLogin = false)
    {
      DataManager.clm = CacheLocationManager.getCacheLocationManager(Path.Combine(DataManager.CustomPath, "DigitalArcSystems", "DataManagerCache"));
      string str = CacheLocationManager.whoIsRunning();
      if (str == null)
      {
        EngineLog.Warning("No other RPGSuite apps are running.");
        DataManager.DoLogIn(settings, loginCallback, forceOfflineLogin);
      }
      else
      {
        EngineLog.Error("Someone else is running. Try again later");
        throw new MultipleDataManagerException("Another RPGSuite Application (" + str + ") is already running.  Please close all other RPGSuite Apps.");
      }
    }

    private static void DoLogIn(
      ClientSettings settings,
      Action<DataManager, UserAccessRecord, string> loginCallback,
      bool forceOfflineLogin = false)
    {
      DataManager dm = DataManager.Instance;
      if (dm == null)
        Debug.Write("DATA MANAGER IS NULL");
      switch (settings.LogonType)
      {
        case LogonType.Facebook:
          dm._siteAPI = (Api) FacebookUser.Instance;
          break;
        case LogonType.Twitter:
          dm._siteAPI = (Api) TwitterUser.Instance;
          break;
        default:
          Console.WriteLine("Using WordPressUser.Instance");
          WordPressUser.Instance.Username = settings.WordPressUsername;
          WordPressUser.Instance.Password = settings.WordPressPassword;
          dm._siteAPI = (Api) WordPressUser.Instance;
          Console.WriteLine("Got dm.siteAPI Instance");
          break;
      }
      if (dm.Configuration == null)
      {
        Console.WriteLine("DM crashed on deserializing cache. Uh oh.");
        dm.Log("DM crashed on deserializing cache. Uh oh.");
        Console.WriteLine("DM online status: {0}", (object) dm.OnlineStatus);
        dm.Log("DM online status: {0}", (object) dm.OnlineStatus.ToString());
        loginCallback((DataManager) null, (UserAccessRecord) null, "An error occured in cache deserialization.");
      }
      else
      {
        string dmCacheDirectory = dm.DM_CacheDirectory;
        if (File.Exists(Path.Combine(dmCacheDirectory, "staging")))
        {
          dm._siteAPI.UseTestServer();
          dm._siteAPI.Log("Using the staging server");
        }
        else
        {
          dm._siteAPI.UseProductionServer();
          dm._siteAPI.Log("Using the production server");
        }
        string previousLoginPath = Path.Combine(dmCacheDirectory, settings.WordPressUsername.SHA1());
        dm.OnlineStatus = DataManager.LogonStatus.Authenticating;
        try
        {
          DateTime now = DateTime.Now;
          Console.WriteLine("About to call the API Login() method");
          dm._siteAPI.Log("About to call the API Login() method");
          dm._siteAPI.Login((Action<bool, UserAccessRecord>) ((success, token) =>
          {
            Console.WriteLine("Inside _siteAPI.Login call back");
            dm._siteAPI.Log(string.Format("API Login() callback: {0}. Elapsed time: {1} ms", success ? (object) "Successful login" : (object) "Unsuccessful login", (object) Math.Round((DateTime.Now - now).TotalMilliseconds)));
            if (!forceOfflineLogin & success && !string.IsNullOrEmpty(token?.UserRecord?.display_name))
              DataManager.OnlineLoginSychronize(settings, loginCallback, dm, token, previousLoginPath);
            else if (File.Exists(previousLoginPath))
            {
              DataManager.OfflineLogin(settings, loginCallback, previousLoginPath, dm);
            }
            else
            {
              dm.Log("Login error, returning null");
              dm.OnlineStatus = DataManager.LogonStatus.Error;
              dm.Log(string.Format("DM online status: {0}", (object) dm.OnlineStatus));
              loginCallback((DataManager) null, (UserAccessRecord) null, "Sorry, unable to log in.");
            }
          }));
        }
        catch (Exception ex)
        {
          dm.Log("caught API exception: " + ex.Message + "\n" + ex.StackTrace);
        }
      }
    }

    private static void OnlineLoginSychronize(
      ClientSettings settings,
      Action<DataManager, UserAccessRecord, string> loginCallback,
      DataManager dm,
      UserAccessRecord token,
      string previousLoginPath)
    {
      Console.WriteLine("Current user access token: " + dm._siteAPI.UserAccessToken.access_token);
      DataManager.clm.startHeartBeat();
      dm.Log("Current user access token: " + dm._siteAPI.UserAccessToken.access_token);
      dm.OnlineStatus = DataManager.LogonStatus.Authenticated;
      settings.ID = token.UserRecord.ID;
      DataManager.UserID = token.UserRecord.ID;
      QueuedWriter.UserID = 0;
      File.WriteAllBytes(previousLoginPath, settings.AsByteArray().EncryptCloudAsset());
      Console.WriteLine("Updating cache from cloud");
      dm.Log("Updating cache from cloud ");
      new Thread(new ParameterizedThreadStart(dm.WaitForNetworkActivityToComplete))
      {
        IsBackground = false
      }.Start();
      Console.WriteLine("Started Wait for Network Activity Thread To Complete");
      new Thread((ParameterizedThreadStart) (x =>
      {
        dm.UpdateNonLicensedCache();
        while (!dm._nonlicensedDownloadStart)
          Thread.Sleep(250);
        Thread.Sleep(1000);
        while (dm._nonLicensedDownloadCount != 0)
          Thread.Sleep(250);
        dm.Log("NO LONGER WAITING FOR UNLICENSED TO DOWNLOAD");
        dm.UpdateLicensedAssetCache();
      }))
      {
        IsBackground = true,
        Priority = ThreadPriority.Lowest
      }.Start();
      dm.Log(string.Format("DM online status: {0}", (object) dm.OnlineStatus));
      loginCallback(dm, token, "Welcome, " + token.UserRecord.display_name + "!");
    }

    private static void OfflineLogin(
      ClientSettings settings,
      Action<DataManager, UserAccessRecord, string> loginCallback,
      string previousLoginPath,
      DataManager dm)
    {
      ClientSettings clientSettings = Encoding.UTF8.GetString(File.ReadAllBytes(previousLoginPath).DecryptCloudAsset()).AsDeserialized<ClientSettings>();
      if (clientSettings.WordPressUsername == settings.WordPressUsername && clientSettings.WordPressPassword == settings.WordPressPassword)
      {
        dm.OnlineStatus = DataManager.LogonStatus.Offline;
        DataManager.UserID = clientSettings.ID;
        dm.Log(string.Format("DM online status: {0}", (object) dm.OnlineStatus));
        DataManager.clm.startHeartBeat();
        loginCallback(dm, new UserAccessRecord(), "Welcome, " + settings.WordPressUsername + "! Continuing in offline mode.");
        dm.OnReady(new EventArgs());
      }
      else
      {
        dm.OnlineStatus = DataManager.LogonStatus.Rejected;
        dm.Log(string.Format("DM online status: {0}", (object) dm.OnlineStatus));
        loginCallback((DataManager) null, (UserAccessRecord) null, "Sorry, login name or password not recognized");
      }
    }

    private List<S3AssetMetadata> GetBasicAssetGuids()
    {
      Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DataManager.BASIC_ASSETS_JSON);
      List<string> source;
      using (StreamReader reader1 = new StreamReader(manifestResourceStream))
      {
        using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
          source = new JsonSerializer().Deserialize<List<string>>((JsonReader) reader2);
      }
      return source.Select<string, S3AssetMetadata>((Func<string, S3AssetMetadata>) (guid => new S3AssetMetadata(new Guid(guid), ""))).ToList<S3AssetMetadata>();
    }

    private void WaitForNetworkActivityToComplete(object state)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      while (!(flag1 & flag2 & flag3))
      {
        Thread.Sleep(100);
        if (this._licensedStart && Interlocked.CompareExchange(ref this._licensedAssetDownloadCount, 0, 0) == 0)
          flag1 = true;
        if (this._nonlicensedDownloadStart && Interlocked.CompareExchange(ref this._nonLicensedDownloadCount, 0, 0) == 0)
          flag2 = true;
        if (this._nonlicensedUploadStart && Interlocked.CompareExchange(ref this._nonLicensedUploadCount, 0, 0) == 0)
          flag3 = true;
      }
      this.OnDownloadComplete(new DownloadCompleteEventArgs(AssetType.NonLicensed));
      this.OnDownloadComplete(new DownloadCompleteEventArgs(AssetType.Licensed));
      this.OnUploadComplete(new UploadCompleteEventArgs());
      this.CleanupCache();
    }

    public bool OnClosing()
    {
      try
      {
        if (DataManager.clm != null)
          DataManager.clm.endHeartBeat();
        else if (!DataManager._isAssetAuthoringDataManager)
          EngineLog.Warning("DM is closing but there is no CLM to end hearbeat with.");
        this.SaveConfiguration(false);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void Save<T>(T item, bool andUpload = false) where T : INonLicensedAsset
    {
      string str = Path.Combine(this.DM_CacheDirectory, item.DefaultFileName);
      this.AssetSerializerHandler((object) item, str);
      this.Export((object) item, str);
      if (andUpload)
        this.Upload((IEnumerable<INonLicensedAsset>) new List<INonLicensedAsset>()
        {
          (INonLicensedAsset) item
        });
      this.SaveConfiguration();
    }

    public void Save<T>(IEnumerable<T> items, bool andUpload = false) where T : INonLicensedAsset
    {
      foreach (T obj in items)
        this.Save<T>(obj, andUpload);
    }

    public void SaveLicensedBinaryAsset<T>(T item, string customPath = null) where T : LicensedBinaryAsset
    {
      if (!string.IsNullOrEmpty(customPath))
        item.Save(customPath);
      else
        item.Save("");
    }

    public void SaveLicensedBinaryAssets<T>(IEnumerable<T> items) where T : LicensedBinaryAsset
    {
      foreach (T obj in items)
        this.SaveLicensedBinaryAsset<T>(obj);
    }

    private void Upload(IEnumerable<INonLicensedAsset> items)
    {
      new Thread((ParameterizedThreadStart) (o => this.UploadInternal((object) items)))
      {
        IsBackground = false
      }.Start();
    }

    private void UploadInternal(object arg)
    {
      Console.WriteLine("Uploading assets.");
      if (this.OnlineStatus != 0)
      {
        this.Log("A cloud API method was called, but the user was not logged in.");
      }
      else
      {
        List<INonLicensedAsset> nonLicensedAssetList = arg is IEnumerable<INonLicensedAsset> ? ((IEnumerable<INonLicensedAsset>) arg).ToList<INonLicensedAsset>() : throw new ApplicationException("Argument is of the wrong type!");
        try
        {
          List<DataManager.CompressedUpload> source = new List<DataManager.CompressedUpload>();
          foreach (INonLicensedAsset instance in nonLicensedAssetList)
          {
            DataManager.CompressedUpload compressedUpload = new DataManager.CompressedUpload()
            {
              Original = instance,
              Id = instance.Id,
              Compressed = instance.AsByteArray().Compress()
            };
            source.Add(compressedUpload);
          }
          int totalbytes = source.Sum<DataManager.CompressedUpload>((Func<DataManager.CompressedUpload, int>) (item => item.Compressed.Length));
          int bytesUploaded = 0;
          this.OnUploadProgress(new UploadProgressEventArgs(0L, (long) totalbytes, (string) null, (string) null));
          if (nonLicensedAssetList.Count <= 0)
            return;
          this._nonLicensedUploadCount = source.Count;
          foreach (DataManager.CompressedUpload compressedUpload in source)
          {
            DataManager.CompressedUpload item = compressedUpload;
            string hash = item.Original.SHA1<INonLicensedAsset>();
            Console.WriteLine("Final Recalculation of Hash is: " + hash);
            this.Log("Final Recalculation of Hash is: " + hash);
            byte[] compressed = item.Compressed;
            try
            {
              DataManager.CompressedUpload item1 = item;
              Console.WriteLine("SENDING ITEM ID: " + item1.Id.ToString() + ", HASH: " + hash);
              this._siteAPI.UploadUserAsset(item.Id, hash, compressed, (Action<Guid, bool>) ((id, result) =>
              {
                if (result)
                {
                  this.Log("uploaded item has sha   " + hash);
                  Console.WriteLine("oploaded id {0}", (object) item.Id);
                  bytesUploaded += compressed.Length;
                  this.OnUploadProgress(new UploadProgressEventArgs((long) bytesUploaded, (long) totalbytes, ((IDescribable) item1.Original).Name, (string) null));
                }
                else
                {
                  this.Log("Asset with id {0} failed to upload");
                  Console.WriteLine("Asset with id {0} failed to upload", (object) item.Id);
                  this.OnUploadProgress(new UploadProgressEventArgs((long) bytesUploaded, (long) totalbytes, (string) null, ((IDescribable) item1.Original).Name + "failed to upload"));
                }
                Interlocked.Decrement(ref this._nonLicensedUploadCount);
              }));
            }
            catch (Exception ex)
            {
              this.Log("UploadInternal: " + ex.Message);
            }
          }
        }
        catch (Exception ex)
        {
          this.Log("UploadInternal: " + ex.Message + "\n" + ex.StackTrace);
        }
      }
    }

    public DMConfiguration Configuration { get; private set; }

    private static void TCGLog(string msg, bool isThisWrong = false)
    {
      string message = "DataManager: " + msg;
      if (isThisWrong)
        EngineLog.Warning(message);
      else
        EngineLog.Print(message);
    }

    private void AssetSerializerHandler(
      object item,
      string filePath = "",
      bool saveAfter = true,
      DateTime? lastModified = null)
    {
      if (item is LicensedBinaryAsset)
      {
        ((LicensedBinaryAsset) item).Save();
        filePath = ((BinaryAssetBase) item).ClientFilePath;
      }
      if (!(item is IAsset) && !(item is List<IAsset>))
        return;
      if (item is IAsset)
        this.ProcessIAssetItem((IAsset) item, filePath, lastModified);
      if (item is List<IAsset>)
      {
        foreach (IAsset asset in item as List<IAsset>)
          this.ProcessIAssetItem(asset, filePath, lastModified);
      }
      if (saveAfter)
        this.SaveConfiguration();
    }

    internal void Log(string format, params object[] args)
    {
      if (this._siteAPI != null)
      {
        this._siteAPI.Log(string.Format("{0:dd-MMM-yyyy hh:mm:ss.fff} ", (object) DateTime.Now));
        this._siteAPI.Log(args.Length == 0 ? format : string.Format(format, args));
      }
      DataManager.TCGLog(string.Format("{0:dd-MMM-yyyy hh:mm:ss.fff} ", (object) DateTime.Now));
      DataManager.TCGLog(args.Length == 0 ? format : string.Format(format, args));
    }

    private void ConsumeEmbeddedResourceAssets()
    {
      foreach (string manifestResourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
        this.ImportAssetFromNamedStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceName) ?? throw new NullReferenceException("embedded resource stream is null"), manifestResourceName, true);
    }

    internal void ImportAssetsFromDirectory(
      string directoryPath,
      string searchPattern = "*.*",
      SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
      foreach (string file in Directory.GetFiles(directoryPath, searchPattern, searchOption))
        this.ImportAssetFromFile(file, true);
    }

    internal void ImportAssetFromFile(string fileName, bool skipUnknownFileTypes = false)
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        this.ImportAssetFromNamedStream((Stream) fileStream, fileName, skipUnknownFileTypes);
    }

    private void ImportAssetFromNamedStream(
      Stream stream,
      string streamName,
      bool skipUnknownAssetTypes = false)
    {
      if (streamName.EndsWith(".tcjson"))
        this.AssetSerializerHandler((object) stream.FromStream<Career>());
      else if (streamName.EndsWith(".secjson"))
        this.AssetSerializerHandler((object) stream.FromStream<Sector>());
      else if (streamName.EndsWith(".trjson"))
        this.AssetSerializerHandler((object) stream.FromStream<Race>());
      else if (streamName.EndsWith(".emjson") || streamName.EndsWith(".tejson") || streamName.Contains("talents.ttjson") || streamName.Contains("skills.skjson"))
        this.AssetSerializerHandler((object) stream.FromStream<List<IAsset>>());
      else if (!skipUnknownAssetTypes)
        throw new FormatException("Unable to import asset from stream " + streamName + ": Unknown asset type name");
    }

    private void CreateConfiguration()
    {
      Console.WriteLine("Create Configuration Started");
      this.Log("Create Configuration Started.");
      string dmCacheDirectory = this.DM_CacheDirectory;
      DirectoryInfo rootDirectory = new DirectoryInfo(dmCacheDirectory);
      Console.WriteLine("Creating config file in " + dmCacheDirectory);
      this.Log("Creating config file in " + dmCacheDirectory);
      try
      {
        this.CreateConfiguration(rootDirectory);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Create configuration at " + dmCacheDirectory + " failed. Error: " + ex.Message);
        this.Log("Create configuration at " + dmCacheDirectory + " failed. Error: " + ex.Message);
      }
      File.Create(this._lockfile).Close();
      if (DataManager._isAssetAuthoringDataManager)
        this.ConsumeEmbeddedResourceAssets();
      File.Delete(this._lockfile);
      Console.WriteLine("Saving Configuration");
      this.Log("Saving Configuration");
      this.SaveConfiguration(false);
    }

    private void CreateConfiguration(DirectoryInfo rootDirectory)
    {
      this.Log("About to create Directories");
      Console.WriteLine("About to create Directories");
      this.Configuration = new DMConfiguration()
      {
        Cache = new Dictionary<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>()
      };
      FieldInfo[] fields = typeof (DMConfiguration).GetFields();
      Console.WriteLine("Found " + fields.Length.ToString() + " directories to write into the configuration file");
      Console.WriteLine("Directories: ");
      foreach (FieldInfo fieldInfo in fields)
      {
        Console.WriteLine("Dir: " + fieldInfo.Name);
        if (fieldInfo.Name.Contains("Directory"))
        {
          try
          {
            DirectoryInfo directoryInfo = new DirectoryInfo((string) fieldInfo.GetValue((object) this.Configuration));
            this.Log("Creating " + directoryInfo?.ToString());
            Console.WriteLine("Creating " + directoryInfo?.ToString());
            if (!directoryInfo.Exists)
              directoryInfo.Create();
            this.Log(" [SUCCESS]");
            Console.WriteLine(" [SUCCESS]");
          }
          catch (Exception ex)
          {
            this.Log(ex.ToString());
            this.Log(ex.StackTrace);
            this.Log(" [FAILED]");
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(" [FAILED]");
          }
        }
      }
    }

    private void DeleteLicensedAssets(IEnumerable<IAssetBase> serializableAssets)
    {
      foreach (IAssetBase serializableAsset in serializableAssets)
        this.DeleteLocal(serializableAsset);
      this.SaveConfiguration();
    }

    internal void DeleteLocal(IAssetBase asset, bool save = false)
    {
      if (asset == null)
        return;
      com.digitalarcsystems.Traveller.DataModel.AssetMetadata assetMetadata;
      if (this.Configuration.Cache.TryGetValue(asset.Id, out assetMetadata))
      {
        this.Configuration.Cache.Remove(asset.Id);
        if (!string.IsNullOrEmpty(assetMetadata.Location))
          File.Delete(assetMetadata.Location);
        assetMetadata.Dispose();
        if (File.Exists(assetMetadata.Location))
          File.Delete(assetMetadata.Location);
      }
      if (!save)
        return;
      this.SaveConfiguration();
    }

    private void MaybeUpgradeLocalCacheToNewDataModel()
    {
      if (DataManager._isAssetAuthoringDataManager)
        return;
      ProductLicenseToken productLicenseToken = DataManager.Instance.GetAsset<ProductLicenseToken>((Func<ProductLicenseToken, bool>) (x => x.Name == "Asset Model Version")).FirstOrDefault<ProductLicenseToken>();
      double result = -1.0;
      if (productLicenseToken != null && double.TryParse(productLicenseToken.Description, out result) && result > this.Configuration.DataModelVersion)
      {
        EngineLog.Print("UPGRADING ASSETS TO " + result.ToString());
        Console.Out.WriteLine("UPGRADING ASSETS TO " + result.ToString());
        foreach (KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata> keyValuePair in this.Configuration.Cache.Where<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>>((Func<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>, bool>) (x => x.Value.TheAsset is INonLicensedAsset)))
        {
          if (keyValuePair.Value.TheAsset is Character)
            ((Character) keyValuePair.Value.TheAsset).convertToNewDataModel();
          keyValuePair.Value.TheAsset = (IAssetBase) DataModelTransition.UpgradeChildren((IAsset) keyValuePair.Value.TheAsset);
        }
        this.Configuration.DataModelVersion = result;
        EngineLog.Print("UPGRADE COMPLETE");
        Console.Out.WriteLine("UPGRADE COMPLETE");
      }
      else
      {
        Console.Out.WriteLine("Upgrade not performed.  Current Asset Versions Numeric: " + result.ToString());
        Console.Out.WriteLine("Current Asset Version String: " + (productLicenseToken != null ? productLicenseToken.Description : "NULL"));
        Console.Out.WriteLine("Upgrade not performed.  Current Asset Versions Numeric: " + result.ToString());
        Console.Out.WriteLine("Current Asset Version String: " + (productLicenseToken != null ? productLicenseToken.Description : "NULL"));
      }
    }

    private void Initialize()
    {
      Console.WriteLine(string.Format("Inside Initialize [{0}]", (object) ++this._numTimesInitializedCalled));
      string dmCacheDirectory = this.DM_CacheDirectory;
      Console.WriteLine("DM_CacheDirectory: " + dmCacheDirectory);
      if (!Directory.Exists(dmCacheDirectory))
      {
        Console.WriteLine("cache directory -- " + dmCacheDirectory + " -- doesn't exist, so creating it now");
        Directory.CreateDirectory(dmCacheDirectory);
      }
      string str1 = Path.Combine(dmCacheDirectory, "tcg_config.conf");
      string str2 = Path.Combine(dmCacheDirectory, "tcg_config.bak");
      if (File.Exists(this._lockfile))
      {
        Console.WriteLine("Error: Configuration was not cleanly created in DataManagerCache.  Did something crash last time?");
        this.Log("Error: Configuration was not cleanly created in DataManagerCache.  Did something crash last time?", (object) true);
        File.Delete(this._lockfile);
      }
      if (!File.Exists(str1) && !File.Exists(str2))
      {
        Console.WriteLine("First Run");
        this.Log("First Run");
        this.CreateConfiguration();
      }
      else if (!File.Exists(str1) && File.Exists(str2))
        File.Copy(str2, str1);
      try
      {
        DateTime now = DateTime.Now;
        Console.WriteLine("About to load cache from " + str1);
        this.Configuration = this.Load<DMConfiguration>(str1);
        Console.WriteLine("Loaded cache");
        Debug.WriteLine("LOCATION: " + str1);
        this.Configuration.Cache = this.Configuration.Cache.Where<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>>((Func<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>, bool>) (q => q.Value != null)).ToDictionary<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>, Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>((Func<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>, Guid>) (q => q.Key), (Func<KeyValuePair<Guid, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>, com.digitalarcsystems.Traveller.DataModel.AssetMetadata>) (q => q.Value));
        Debug.WriteLine(string.Format("Cache deserialization took {0}s", (object) (DateTime.Now - now).TotalSeconds));
      }
      catch (Exception ex)
      {
        Console.WriteLine("error on cache deserialization: " + ex.Message);
        this.Log("error on cache deserialization: " + ex.Message);
        if (DataManager._isAssetAuthoringDataManager)
          throw new ApplicationException("Asset authoring Data manager did not come up cleanly: " + ex.Message);
        try
        {
          if (File.Exists(str1))
            File.Delete(str1);
          if (File.Exists(this._lockfile))
            File.Delete(this._lockfile);
        }
        catch
        {
        }
        this.Initialize();
      }
      LicensedBinaryAsset.Configuration = this.Configuration;
      LocalUnlicensedAssetSynchronizer.synchronize(dmCacheDirectory, this.Configuration.Cache, this);
    }

    internal T Load<T>(string filePath)
    {
      Console.WriteLine("Attempting to load at " + filePath);
      Exception exception = (Exception) null;
      T obj = default (T);
      if (this._encrypt && !((object) default (T) is INonLicensedAsset) || DataManager.DecryptConfigFile && filePath.ToLowerInvariant().Contains("tcg_config.conf"))
      {
        try
        {
          Console.WriteLine("Decrypting " + filePath);
          obj = QueuedWriter.Decrypt(filePath).AsDeserialized<T>();
        }
        catch (Exception ex)
        {
          EngineLog.Error("Unable to read cache because: " + ex.Message);
          exception = ex;
        }
      }
      else
        obj = File.ReadAllText(filePath).AsDeserialized<T>();
      if ((object) obj == null && filePath.ToLowerInvariant().Contains("tcg_config.conf") && !this._encrypt)
      {
        Console.WriteLine("Trying Decrypting " + filePath);
        try
        {
          string instance = QueuedWriter.Decrypt(filePath);
          if (instance != null)
            obj = instance.AsDeserialized<T>();
        }
        catch (Exception ex)
        {
          if (exception == null)
            exception = ex;
        }
      }
      if ((object) obj == null)
      {
        if (filePath.ToLowerInvariant().Contains("tcg_config.conf") && File.Exists(filePath))
        {
          string destFileName = filePath + ".corrupted";
          this.OnErrors(new DataManagerErrorEventArgs("The existing cache " + filePath + " is corrupted. Saving existing cache as " + destFileName));
          Console.WriteLine("The existing cache " + filePath + " is corrupted. Saving existing cache as " + destFileName);
          File.Move(filePath, destFileName);
          if (exception != null)
            throw exception;
        }
        else
          this.OnErrors(new DataManagerErrorEventArgs("An error occurred while loading from " + filePath));
      }
      return obj;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void ProcessIAssetItem(IAsset item, string filePath = "", DateTime? lastModified = null)
    {
      if (!lastModified.HasValue)
        lastModified = new DateTime?(DateTime.UtcNow);
      string str = item.SHA1<IAsset>();
      if (item.Id == Guid.Empty)
        throw new InvalidOperationException("Asset " + item.Name + " has an empty Id.  This operation is not permitted.");
      if (item is INonLicensedAsset && ((INonLicensedAsset) item).CreatingUser == 0)
        throw new InvalidOperationException("Nonlicensed asset " + item.Name + " exists but the creating user is unknown.");
      com.digitalarcsystems.Traveller.DataModel.AssetMetadata assetMetadata1;
      if (this.Configuration.Cache.TryGetValue(item.Id, out assetMetadata1))
      {
        if (!(assetMetadata1.LastModified < DateTime.UtcNow) || !(str != assetMetadata1.StoredHash))
          return;
        this.Configuration.Cache[item.Id].TheAsset = (IAssetBase) item;
        this.Configuration.Cache[item.Id].Initialize();
      }
      else
      {
        com.digitalarcsystems.Traveller.DataModel.AssetMetadata assetMetadata2 = new com.digitalarcsystems.Traveller.DataModel.AssetMetadata()
        {
          StoredHash = str,
          Location = filePath,
          LastModified = lastModified.Value,
          TheAsset = (IAssetBase) item
        };
        assetMetadata2.Initialize();
        this.Configuration.Cache.Add(item.Id, assetMetadata2);
      }
    }

    private void SaveConfiguration(bool saveInBackground = true)
    {
      if (this.Configuration == null || File.Exists(this._lockfile))
        return;
      string pathAndName = Path.Combine(this.DM_CacheDirectory, "tcg_config.conf");
      this.Log("Saving cache to " + pathAndName);
      Console.WriteLine("Saving cache to " + pathAndName);
      this.Export((object) this.Configuration, pathAndName, saveInBackground);
    }

    private void UpdateLicensedAssetCache()
    {
      if (this._ulacNumtries++ < 3)
      {
        if (this.OnlineStatus != 0)
        {
          this.Log("A cloud API method was called, but the user was not logged in.");
        }
        else
        {
          try
          {
            this._licensedAssetDownloadCount = 1;
            this._licensedStart = true;
            EngineLog.Print("Ensuring all basic assets are in cache");
            Console.WriteLine("Ensuring all basic assets are in cache");
            AssetResourceProvider arp = new AssetResourceProvider();
            this._siteAPI.ListLicensedAssets(false, (Action<IEnumerable<S3AssetMetadata>>) (listOfAssets =>
            {
              List<S3AssetMetadata> assets = new List<S3AssetMetadata>(listOfAssets);
              assets.AddRange((IEnumerable<S3AssetMetadata>) this.GetBasicAssetGuids());
              arp.WriteAssetsToCache((IEnumerable<S3AssetMetadata>) assets, this.Configuration.Cache);
            }));
            while (!arp.Finished())
              Thread.Sleep(1000);
            this._licensedAssetDownloadCount = 0;
          }
          catch (APIException ex)
          {
            this.Log("Caught exception listing cloud licensed assets: " + ex.Message);
          }
          catch (JsonSerializationException ex)
          {
            this.Log("caught exception, assets came back null: " + ex.Message);
          }
        }
      }
      else
      {
        this.Log("Unable to download Licensed Assets.  Continuing without them");
        this._licensedStart = true;
        this._licensedAssetDownloadCount = 0;
      }
    }

    internal void UpdateLicensedAssetCache(IEnumerable<S3AssetMetadata> assets)
    {
      if (assets == null)
      {
        this.UpdateLicensedAssetCache();
      }
      else
      {
        assets = (IEnumerable<S3AssetMetadata>) assets.ToList<S3AssetMetadata>();
        if (assets.Any<S3AssetMetadata>())
        {
          List<S3AssetMetadata> list = assets.ToList<S3AssetMetadata>();
          List<S3AssetMetadata> source = new List<S3AssetMetadata>();
          int num = 0;
          foreach (S3AssetMetadata s3AssetMetadata in list)
          {
            if (this.Configuration.Cache.ContainsKey(s3AssetMetadata.AssetGuid))
            {
              com.digitalarcsystems.Traveller.DataModel.AssetMetadata assetMetadata = this.Configuration.Cache[s3AssetMetadata.AssetGuid];
              TimeSpan timeSpan = s3AssetMetadata.LastModified - assetMetadata.LastModified.Date;
              if (assetMetadata.StoredHash != s3AssetMetadata.SHA1)
                ++num;
              if (assetMetadata.LastModified.Date < s3AssetMetadata.LastModified.Date && Math.Abs(timeSpan.Days) >= 1 && assetMetadata.StoredHash != s3AssetMetadata.SHA1)
              {
                string name = assetMetadata.TheAsset.Id.ToString();
                if (assetMetadata.TheAsset is IDescribable)
                  name = ((IDescribable) assetMetadata.TheAsset).Name;
                this.Log(string.Format("Cloud Licensed Asset [{0}] will be Downloaded: Cached LM: {1} < {2}", (object) name, (object) assetMetadata.LastModified, (object) s3AssetMetadata.LastModified));
                source.Add(s3AssetMetadata);
              }
            }
            else
              source.Add(s3AssetMetadata);
          }
          this.Log("========= NUM ASSETS WITH DIFFERING HASH ===== [" + num.ToString() + "]============");
          this.SyncLicensedAssets(source.ToList<S3AssetMetadata>());
        }
        else
        {
          this.Log("No Licensed assets to download.");
          this._licensedStart = true;
          this._licensedAssetDownloadCount = 0;
        }
      }
    }

    public event EventHandler<DownloadCompleteEventArgs> DownloadComplete;

    private void OnDownloadComplete(DownloadCompleteEventArgs e)
    {
      lock (this._onDownloadCompleteLock)
      {
        if (e.AssetType == AssetType.Licensed)
          DataManager.IsDownloadingLicensed = false;
        else
          DataManager.IsDownloadingNonLicensed = false;
        EventHandler<DownloadCompleteEventArgs> downloadComplete = this.DownloadComplete;
        if (downloadComplete == null)
          return;
        foreach (EventHandler<DownloadCompleteEventArgs> invocation in downloadComplete.GetInvocationList())
          invocation((object) this, e);
      }
    }

    public static bool IsDownloading
    {
      get => DataManager.IsDownloadingLicensed || DataManager.IsDownloadingNonLicensed;
    }

    public static bool IsTransmittingData => DataManager.IsUploading || DataManager.IsDownloading;

    public static bool IsTransmittingNonLicensed
    {
      get => DataManager.IsUploading || DataManager.IsDownloadingNonLicensed;
    }

    public event EventHandler<UploadCompleteEventArgs> UploadComplete;

    private void OnUploadComplete(UploadCompleteEventArgs e)
    {
      lock (this._onUploadCompleteLock)
      {
        DataManager.IsUploading = false;
        EventHandler<UploadCompleteEventArgs> uploadComplete = this.UploadComplete;
        if (uploadComplete == null)
          return;
        foreach (EventHandler<UploadCompleteEventArgs> invocation in uploadComplete.GetInvocationList())
          invocation((object) this, e);
      }
    }

    public static event EventHandler Errors;

    private void OnErrors(DataManagerErrorEventArgs e)
    {
      lock (this._onErrorsLock)
      {
        EventHandler errors = DataManager.Errors;
        if (errors == null)
          return;
        foreach (EventHandler invocation in errors.GetInvocationList())
          invocation((object) this, (EventArgs) e);
      }
    }

    public static event EventHandler Ready;

    private void OnReady(EventArgs e)
    {
      lock (this._onReadyLock)
      {
        DataManager.IsUploading = false;
        EventHandler ready = DataManager.Ready;
        if (ready == null)
          return;
        foreach (EventHandler invocation in ready.GetInvocationList())
          invocation((object) this, e);
      }
    }

    public event EventHandler<UploadProgressEventArgs> UploadProgress;

    private void OnUploadProgress(UploadProgressEventArgs e)
    {
      lock (this._onUploadLock)
      {
        DataManager.IsUploading = true;
        EventHandler<UploadProgressEventArgs> uploadProgress = this.UploadProgress;
        if (uploadProgress == null)
          return;
        foreach (EventHandler<UploadProgressEventArgs> invocation in uploadProgress.GetInvocationList())
          invocation((object) this, e);
      }
    }

    public event EventHandler<DownloadProgressEventArgs> DownloadProgress;

    internal void OnDownloadProgressChanged(DownloadProgressEventArgs e)
    {
      lock (this._onDownloadProgressLock)
      {
        if (e.AssetType == AssetType.Licensed)
          DataManager.IsDownloadingLicensed = true;
        else
          DataManager.IsDownloadingNonLicensed = true;
        EventHandler<DownloadProgressEventArgs> downloadProgress = this.DownloadProgress;
        if (downloadProgress == null)
          return;
        foreach (EventHandler<DownloadProgressEventArgs> invocation in downloadProgress.GetInvocationList())
          invocation((object) this, e);
      }
    }

    private void SyncLicensedAssets(List<S3AssetMetadata> s3AssetsToDownload)
    {
      long totalBytes = s3AssetsToDownload.Sum<S3AssetMetadata>((Func<S3AssetMetadata, long>) (q => q.Size));
      for (int index = 0; index < s3AssetsToDownload.Count; ++index)
      {
        S3AssetMetadata s3AssetMetadata = s3AssetsToDownload[index];
        if (!this.s3Assets.ContainsKey(s3AssetMetadata.DownloadURL))
          this.s3Assets.Add(s3AssetMetadata.DownloadURL, s3AssetMetadata);
        else
          this.s3Assets[s3AssetMetadata.DownloadURL] = s3AssetMetadata;
      }
      this._licensedStart = true;
      long currentByteCount = 0;
      this._licensedAssetDownloadCount = s3AssetsToDownload.Count;
      int saveInterval = 10;
      double nextPercent = 10.0;
      if (s3AssetsToDownload.Count <= 0)
        return;
      double currentPercent;
      if (this.Configuration.LastPercentageLicensedSynced >= 100.0)
      {
        this.Configuration.LastPercentageLicensedSynced = 0.0;
        currentPercent = (double) currentByteCount / (double) totalBytes * 100.0;
      }
      else
        currentPercent = this.Configuration.LastPercentageLicensedSynced;
      this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, (string) null, (string) null, AssetType.Licensed));
      Api.downloadProgressUpdated += (Action<string, float>) ((url, progress) =>
      {
        S3AssetMetadata s3AssetMetadata = (S3AssetMetadata) null;
        if (!this.s3Assets.TryGetValue(url, out s3AssetMetadata))
          return;
        this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount + (long) ((double) s3AssetMetadata.Size * (double) progress), totalBytes, (string) null, (string) null, AssetType.Licensed));
      });
      foreach (S3AssetMetadata s3AssetMetadata1 in s3AssetsToDownload)
      {
        S3AssetMetadata s3AssetMetadata = s3AssetMetadata1;
        S3AssetMetadata metadata = s3AssetMetadata;
        this.DownloadLicensed(s3AssetMetadata, (Action<S3AssetMetadata, IAsset>) ((s3Asset, newasset) =>
        {
          if (s3Asset != null && newasset != null)
          {
            currentByteCount += s3Asset.Size;
            this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, newasset.Name, (string) null, AssetType.Licensed));
            this.AssetSerializerHandler((object) newasset, saveAfter: false, lastModified: new DateTime?(s3AssetMetadata.LastModified));
            double num = this.Configuration.LastPercentageLicensedSynced / (double) totalBytes + (double) currentByteCount / (double) totalBytes * 100.0;
            if (num > currentPercent)
              currentPercent = num;
          }
          else
          {
            this.Log(string.Format("Tried to download {0} but failed", (object) metadata.AssetGuid));
            this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, (string) null, "Maximum retry exceeded", AssetType.Licensed));
          }
          Interlocked.Decrement(ref this._licensedAssetDownloadCount);
          this.Log(string.Format("Licensed assets left: {0}", (object) this._licensedAssetDownloadCount));
          Console.WriteLine(string.Format("Current Download at {0}%", (object) currentPercent));
          this.Log(string.Format("Current Download at {0}%", (object) currentPercent));
          if (currentPercent < nextPercent)
            return;
          this.Log(string.Format("SAVING CACHE AT {0}%", (object) currentPercent));
          Console.WriteLine(string.Format("SAVING CACHE AT {0}%", (object) currentPercent));
          nextPercent = currentPercent - currentPercent % (double) saveInterval + (double) saveInterval;
          this.SaveConfiguration();
          if (currentPercent > this.Configuration.LastPercentageLicensedSynced)
            this.Configuration.LastPercentageLicensedSynced = currentPercent;
          this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, "(backing up download ...)", (string) null, AssetType.Licensed));
        }));
      }
    }

    internal void AddOrUpdateAsset(IAssetBase asset) => this.AssetSerializerHandler((object) asset);

    private void UpdateNonLicensedCache()
    {
      if (this.OnlineStatus != 0)
      {
        this.Log("A cloud API method was called, but the user was not logged in.");
        Console.WriteLine("A cloud API method was called, but the user was not logged in.");
      }
      else
        this._siteAPI.ListUserAssets(true, (Action<IEnumerable<S3AssetMetadata>>) (a =>
        {
          if (a != null)
          {
            this.Log("Inside the ListUserAsset(API) callback");
            Console.WriteLine("Inside the ListUserAsset(API) callback");
            List<S3AssetMetadata> list = a.ToList<S3AssetMetadata>();
            Console.WriteLine("Found " + a.Count<S3AssetMetadata>().ToString() + " user assets in the cloud. ");
            List<INonLicensedAsset> assetsToUpload = Sync.GetAssetsToUpload((IEnumerable<S3AssetMetadata>) list, DataManager.UserID);
            Console.WriteLine("Removing " + assetsToUpload.Count<INonLicensedAsset>().ToString() + " assets which need to be uploaded.");
            list.RemoveAll((Predicate<S3AssetMetadata>) (x => assetsToUpload.Any<INonLicensedAsset>((Func<INonLicensedAsset, bool>) (y => y.Id == x.AssetGuid))));
            Console.WriteLine("After removing assets that need to be uploaded, " + assetsToUpload.Count<INonLicensedAsset>().ToString() + " remain to download.");
            this._nonlicensedUploadStart = true;
            this.Upload((IEnumerable<INonLicensedAsset>) assetsToUpload);
            List<S3AssetMetadata> assetsToDownload = Sync.GetAssetsToDownload(list);
            long totalBytes = assetsToDownload.Select<S3AssetMetadata, long>((Func<S3AssetMetadata, long>) (asset => asset.Size)).Sum();
            long currentByteCount = 0;
            this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, (string) null, (string) null, AssetType.NonLicensed));
            this._nonlicensedDownloadStart = true;
            if (assetsToDownload.Count <= 0)
              return;
            this._nonLicensedDownloadCount = assetsToDownload.Count;
            double nextPercent = 10.0;
            int saveInterval = 10;
            double currentPercent = Math.Abs(this.Configuration.LastPercentageNonLicensedSynced - 100.0) <= 0.001 ? this.Configuration.LastPercentageNonLicensedSynced : (double) currentByteCount / (double) totalBytes * 100.0;
            foreach (S3AssetMetadata s3AssetMetadata1 in assetsToDownload)
            {
              S3AssetMetadata s3AssetMetadata = s3AssetMetadata1;
              long size = s3AssetMetadata.Size;
              this.DownloadNonlicensed(s3AssetMetadata.DownloadURL, (Action<IAsset>) (x =>
              {
                if (x != null)
                {
                  currentByteCount += size;
                  if (((INonLicensedAsset) x).CreatingUser == 0)
                    ((INonLicensedAsset) x).CreatingUser = DataManager.UserID;
                  this.AssetSerializerHandler((object) x, saveAfter: false, lastModified: new DateTime?(s3AssetMetadata.LastModified));
                  this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, x.Name, (string) null, AssetType.NonLicensed));
                }
                else
                {
                  this.Log(string.Format("nonlicensed asset {0} failed to download", (object) s3AssetMetadata.AssetGuid));
                  this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, (string) null, "Maximum retries exceeded", AssetType.NonLicensed));
                }
                Interlocked.Decrement(ref this._nonLicensedDownloadCount);
                this.Log(string.Format("Nonlicensed assets left: {0}", (object) this._nonLicensedDownloadCount));
                if (currentPercent < nextPercent)
                  return;
                nextPercent = currentPercent - currentPercent % (double) saveInterval + (double) saveInterval;
                this.SaveConfiguration();
                if (currentPercent > this.Configuration.LastPercentageNonLicensedSynced)
                  this.Configuration.LastPercentageNonLicensedSynced = currentPercent;
                this.OnDownloadProgressChanged(new DownloadProgressEventArgs(currentByteCount, totalBytes, "(backing up download ...)", (string) null, AssetType.NonLicensed));
              }));
            }
          }
          else
          {
            this.Log("User assets list was null. Continuing without them");
            this._nonlicensedDownloadStart = true;
            this._nonLicensedDownloadCount = 0;
          }
        }));
    }

    private void CleanupCache()
    {
      lock (this._cleanupLock)
      {
        List<IAsset> localLicensed = this.GetAsset<IAsset>((Func<IAsset, bool>) (x => x is ILicensedAsset)).ToList<IAsset>();
        List<IAsset> localNonLicensed = this.GetAsset<IAsset>((Func<IAsset, bool>) (x => x is INonLicensedAsset)).ToList<IAsset>();
        this._siteAPI.ListLicensedAssets(true, (Action<IEnumerable<S3AssetMetadata>>) (assets =>
        {
          if (assets != null)
          {
            List<S3AssetMetadata> basicAssetGuids = this.GetBasicAssetGuids();
            List<S3AssetMetadata> cloudAssets = assets.ToList<S3AssetMetadata>();
            cloudAssets.AddRange((IEnumerable<S3AssetMetadata>) basicAssetGuids);
            this.DeleteLicensedAssets(localLicensed.Where<IAsset>((Func<IAsset, bool>) (localAsset => !cloudAssets.Select<S3AssetMetadata, Guid>((Func<S3AssetMetadata, Guid>) (x => x.AssetGuid)).Contains<Guid>(localAsset.Id))).Select<IAsset, IAssetBase>((Func<IAsset, IAssetBase>) (localAsset => (IAssetBase) localAsset)));
          }
          DataManager.IsLicensedClean = true;
          this.FireWhenCacheIsSynced();
        }));
        this._siteAPI.ListUserAssets(true, (Action<IEnumerable<S3AssetMetadata>>) (assets =>
        {
          if (assets != null)
          {
            List<S3AssetMetadata> cloudAssets = assets.ToList<S3AssetMetadata>();
            this.Delete(localNonLicensed.Where<IAsset>((Func<IAsset, bool>) (localAsset => !cloudAssets.Select<S3AssetMetadata, Guid>((Func<S3AssetMetadata, Guid>) (x => x.AssetGuid)).Contains<Guid>(localAsset.Id) && ((INonLicensedAsset) localAsset).CreatingUser != DataManager.UserID)).Select<IAsset, INonLicensedAsset>((Func<IAsset, INonLicensedAsset>) (localAsset => (INonLicensedAsset) localAsset)));
          }
          DataManager.IsNonLicensedClean = true;
          this.FireWhenCacheIsSynced();
        }));
      }
    }

    private void FireWhenCacheIsSynced()
    {
      lock (this._syncLock)
      {
        if (!DataManager.IsCacheSynched)
          return;
        this.MaybeUpgradeLocalCacheToNewDataModel();
        this.SaveConfiguration();
        this.OnReady(new EventArgs());
      }
    }

    public static bool IsCacheSynched
    {
      get => DataManager.IsLicensedClean && DataManager.IsNonLicensedClean;
    }

    private DataManager(string dataPath)
    {
      Console.WriteLine("DataManager(" + dataPath + ") called.");
      this.DM_CacheDirectory = Path.Combine(dataPath, "DataManagerCache");
      this.OnlineStatus = DataManager.LogonStatus.NeverTried;
      this._writer = QueuedWriter.Instance;
      this._lockfile = Path.Combine(this.DM_CacheDirectory, "creating.lock");
      Console.WriteLine("Initializing Crypto.");
      ExtensionMethods.InitializeCrypto("brMLaU3t9Y20fqSJaOaI9ewph0jBai71", "spMAN1IE2Aw=", "SlVcJWqZeyVJlq6gA3mqoPeUanmHSDIPjN2uob7hqdk=", "UAf3lpK3AUqCvVtQDWOplQ==");
      DateTime now = DateTime.Now;
      Console.WriteLine("About to call initialize");
      this.Initialize();
      Console.WriteLine(string.Format("DataManager.Initialize took {0} seconds", (object) (DateTime.Now - now).TotalSeconds));
    }

    public enum LogonStatus
    {
      Authenticated,
      Authenticating,
      Rejected,
      Expired,
      Error,
      NeverTried,
      Offline,
    }

    private delegate void Del();

    private class CompressedUpload
    {
      public INonLicensedAsset Original;
      public Guid Id;
      public byte[] Compressed;
    }
  }
}
