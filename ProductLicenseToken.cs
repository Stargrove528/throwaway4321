
// Type: com.digitalarcsystems.Traveller.DataModel.ProductLicenseToken




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class ProductLicenseToken : StringAsset, ILicensedAsset, IAssetBase
  {
    [JsonProperty]
    public string ProductId;
    [JsonProperty]
    public string[] AddressableLabels;

    [JsonConstructor]
    public ProductLicenseToken(string name, string description, Guid id, string productId)
    {
      this.Name = name;
      this.Description = description;
      this.Id = id;
      this.ProductId = productId;
      this.AddressableLabels = new string[0];
    }

    public ProductLicenseToken(
      string name,
      string description,
      Guid id,
      string productId,
      params string[] addressableLabel)
      : this(name, description, id, productId)
    {
      this.AddressableLabels = addressableLabel;
    }
  }
}
