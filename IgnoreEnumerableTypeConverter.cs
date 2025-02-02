
// Type: com.digitalarcsystems.Traveller.IgnoreEnumerableTypeConverter




using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public class IgnoreEnumerableTypeConverter : JsonConverter
  {
    public override bool CanConvert(System.Type objectType)
    {
      return objectType.GetDictionaryKeyValueTypes().Count<System.Type[]>() == 1;
    }

    public override object ReadJson(
      JsonReader reader,
      System.Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      existingValue = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
      JObject jobject = JObject.Load(reader);
      jobject.Remove("$type");
      using (JsonReader reader1 = jobject.CreateReader())
        serializer.Populate(reader1, existingValue);
      return existingValue;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
