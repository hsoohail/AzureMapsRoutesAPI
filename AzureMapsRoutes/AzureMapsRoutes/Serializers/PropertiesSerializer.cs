using System.Text.Json;
using System.Text.Json.Serialization;
using AzureMapsRoutes.Models;

namespace AzureMapsRoutes.Serializers;

public class PropertiesJsonConverter : JsonConverter<IProperties>
{
    public override IProperties? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions? options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement root = doc.RootElement;
        
        string type = root.GetProperty("type").GetString();
        
        return type switch
        {
            "Waypoint" => JsonSerializer.Deserialize<WaypointProperties>(root.GetRawText(), options),
            "RoutePath" => JsonSerializer.Deserialize<RoutePathProperties>(root.GetRawText(), options),
            "ManeuverPoint" => JsonSerializer.Deserialize<ManeuverPointProperties>(root.GetRawText(), options),
            _ => new GenericProperties
            {
                Type = type,
                RawJson = root.GetRawText(),
                Data = JsonSerializer.Deserialize<Dictionary<string, object>>(root.GetRawText(), options)
            }
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        IProperties value,
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case WaypointProperties waypoint:
                JsonSerializer.Serialize(writer, waypoint, options);
                break;
            case RoutePathProperties routePath:
                JsonSerializer.Serialize(writer, routePath, options);
                break;
            default:
                throw new JsonException($"Unknown properties type: {value.GetType().Name}");
        }
    }
}