
// Type: com.digitalarcsystems.Traveller.DataModel.LicensedBinaryAsset




using Newtonsoft.Json;
using RPGSuiteCloud;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class LicensedBinaryAsset : BinaryAssetBase, ILicensedAsset, IAssetBase
  {
    [JsonIgnore]
    private byte[] _contents;
    private const uint Magic = 2616705838;

    [JsonIgnore]
    public override byte[] Contents
    {
      get
      {
        if (this._contents != null)
          return this._contents;
        byte[] contents = (byte[]) null;
        try
        {
          LicensedBinaryAsset licensedBinaryAsset = LicensedBinaryAsset.LoadFrom(this.GetPath());
          this.ContentHash = licensedBinaryAsset.ContentHash;
          contents = licensedBinaryAsset._contents;
          licensedBinaryAsset._contents = (byte[]) null;
        }
        catch (Exception ex)
        {
          EngineLog.Error(ex.ToString());
        }
        finally
        {
          GC.Collect();
        }
        return contents;
      }
    }

    public string SourceFilename { get; set; }

    [JsonIgnore]
    public static DMConfiguration Configuration { get; set; }

    public static LicensedBinaryAsset CreateFrom(string path, Guid id)
    {
      byte[] numArray = ASyncFileUtility<LicensedBinaryAsset>.ReadSync(path);
      LicensedBinaryAsset from = new LicensedBinaryAsset();
      from.Id = id;
      from._contents = numArray;
      from.ContentHash = numArray.SHA1();
      from.SourceFilename = path;
      return from;
    }

    public static LicensedBinaryAsset CreateFrom(byte[] bytes)
    {
      LicensedBinaryAsset from = LicensedBinaryAsset.LoadFrom(bytes);
      from.Save();
      return from;
    }

    public static LicensedBinaryAsset LoadFrom(byte[] bytes)
    {
      using (MemoryStream input = new MemoryStream(bytes))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          string str = binaryReader.ReadUInt32() == 2616705838U ? binaryReader.ReadString() : throw new FileLoadException("Licensed file not in expected format.");
          LicensedBinaryAsset licensedBinaryAsset = str.AsDeserialized<LicensedBinaryAsset>();
          licensedBinaryAsset._contents = binaryReader.ReadBytes(bytes.Length - Encoding.UTF8.GetByteCount(str) - 4);
          licensedBinaryAsset.ContentHash = licensedBinaryAsset._contents.SHA1();
          return licensedBinaryAsset;
        }
      }
    }

    public static LicensedBinaryAsset LoadFrom(string path)
    {
      return LicensedBinaryAsset.LoadFrom(ASyncFileUtility<LicensedBinaryAsset>.ReadSync(path));
    }

    public byte[] AsByteArray()
    {
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          string str = this.AsSerialized();
          binaryWriter.Write(2616705838U);
          binaryWriter.Write(str);
          byte[] buffer = this._contents ?? this.Contents;
          if (buffer == null)
            Console.WriteLine("Invalid Contents");
          binaryWriter.Write(buffer);
          return output.ToArray();
        }
      }
    }

    public void Save(string path = "")
    {
      if (string.IsNullOrEmpty(path))
        path = this.GetPath();
      this.ClientFilePath = Path.GetFileName(path);
      File.WriteAllBytes(path, this.AsByteArray());
      this._contents = (byte[]) null;
      GC.Collect();
    }

    private string GetPath()
    {
      return Path.Combine(DataManager.Instance.DM_CacheDirectory, string.Format("{0}{1}", (object) this.Id.ToString().ToLower(), (object) ".trlic"));
    }

    public override string ToString()
    {
      return !string.IsNullOrEmpty(this.SourceFilename) ? Path.GetFileName(this.SourceFilename) : string.Format("LicensedBinaryAsset {0}", (object) this.Id);
    }

    public static bool IsValid(byte[] bytes)
    {
      return bytes[3] == (byte) 155 && bytes[2] == (byte) 247 && bytes[1] == (byte) 195 && bytes[0] == (byte) 46;
    }
  }
}
