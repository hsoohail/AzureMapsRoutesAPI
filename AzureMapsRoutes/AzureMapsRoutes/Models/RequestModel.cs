using System.Text.Json.Serialization;

namespace AzureMapsRoutes.Models;

public class RouteRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "FeatureCollection";
    
    [JsonPropertyName("features")]
    public List<RequestFeature> Features { get; set; } = new List<RequestFeature>();
    
    [JsonPropertyName("optimizeRoute")]
    public string OptimizeRoute { get; set; }
    
    [JsonPropertyName("routeOutputOptions")]
    public List<string> RouteOutputOptions { get; set; } = new List<string>();
    
    [JsonPropertyName("travelMode")]
    public string TravelMode { get; set; }
    
    [JsonPropertyName("optimizeWaypointOrder")]
    public bool OptimizeWaypointOrder { get; set; }
}