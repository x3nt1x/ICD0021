using System.Text.Json;

namespace Tests.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public static TValue? DeserializeWithWebDefaults<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
    }
}