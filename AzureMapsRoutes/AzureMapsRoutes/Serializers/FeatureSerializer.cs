using System.Text.Json;
using System.Text.Json.Serialization;
using AzureMapsRoutes.Models;

namespace AzureMapsRoutes.Serializers;

public class FeatureConverter : JsonConverter<Feature>
{
    public override Feature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        var feature = new Feature
        {
            Type = root.GetProperty("type").GetString(),
            Geometry = JsonSerializer.Deserialize<IGeometry>(root.GetProperty("geometry").GetRawText(), options),
            Properties = JsonSerializer.Deserialize<IProperties>(root.GetProperty("properties").GetRawText(), options)
        };
        
        return feature;
    }

    public override void Write(Utf8JsonWriter writer, Feature value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.Type);
        writer.WritePropertyName("geometry");
        JsonSerializer.Serialize(writer, value.Geometry, options);
        writer.WritePropertyName("properties");
        JsonSerializer.Serialize(writer, value.Properties, options);
        writer.WriteEndObject();
    }
}