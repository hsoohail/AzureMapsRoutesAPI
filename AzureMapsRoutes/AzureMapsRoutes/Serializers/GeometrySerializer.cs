using System.Text.Json;
using System.Text.Json.Serialization;
using AzureMapsRoutes.Models;

namespace AzureMapsRoutes.Serializers;

public class GeometryJsonConverter : JsonConverter<IGeometry>
{
    public override IGeometry? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions? options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement root = doc.RootElement;
        
        string type = root.GetProperty("type").GetString();
        
        return type switch
        {
            "Point" => JsonSerializer.Deserialize<PointGeometry>(root.GetRawText(), options),
            "MultiLineString" => JsonSerializer.Deserialize<MultiLineStringGeometry>(root.GetRawText(), options),
            _ => throw new JsonException($"Unknown geometry type: {type}")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        IGeometry value,
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case PointGeometry point:
                JsonSerializer.Serialize(writer, point, options);
                break;
            case MultiLineStringGeometry multiLine:
                JsonSerializer.Serialize(writer, multiLine, options);
                break;
            default:
                throw new JsonException($"Unknown geometry type: {value.GetType().Name}");
        }
    }
}