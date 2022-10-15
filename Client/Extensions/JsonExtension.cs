using Client.Services.RequestProcessor.RequestModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Client.Extensions;

public static class JsonExtension
{
    private const string Schema = @"{
'code': {'type': 'integer'},
'message': {'type': 'string'},
}";

    private static JSchema JSchema => JSchema.Parse(Schema);

    public static bool TryParseJson<T>(this IResponse json, out T deserialized) where T : new()
    {
        if (string.IsNullOrEmpty(json.Content) || int.TryParse(json.Content, out _))
        {
            deserialized = default;
            return false;
        }
            
        var jObject = JObject.Parse(json.Content);

        var isValid = jObject.IsValid(JSchema);
            
        if (!isValid)
        {
            deserialized = default;
            return false;
        }

        deserialized = JsonConvert.DeserializeObject<T>(json.Content);
        return true;
    }
}