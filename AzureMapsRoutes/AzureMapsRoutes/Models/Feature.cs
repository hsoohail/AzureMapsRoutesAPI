using System.Text.Json.Serialization;

namespace AzureMapsRoutes.Models;

// Root object
public class FeatureCollection
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "FeatureCollection";
    
    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; } = new List<Feature>();
}

// Feature with discriminated properties
public class Feature
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Feature";
    
    [JsonPropertyName("geometry")]
    public IGeometry Geometry { get; set; }
    
    [JsonPropertyName("properties")]
    public IProperties Properties { get; set; }
}

public class RequestFeature
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Feature";
    
    [JsonPropertyName("geometry")]
    public PointGeometry Geometry { get; set; }
    
    [JsonPropertyName("properties")]
    public RequestProperties Properties { get; set; }
}

public class RequestProperties
{
    [JsonPropertyName("pointIndex")]
    public int PointIndex { get; set; }
    
    [JsonPropertyName("pointType")]
    public string PointType { get; set; }
}

#region Geometry Types
public interface IGeometry 
{
    string Type { get; }
    List<double>? Bbox { get; set; }
}

public class PointGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public string Type { get; } = "Point";
    
    [JsonPropertyName("coordinates")]
    public List<double> Coordinates { get; set; } = new List<double>();
    
    [JsonPropertyName("bbox")]
    public List<double>? Bbox { get; set; }
}

public class MultiLineStringGeometry : IGeometry
{
    [JsonPropertyName("type")]
    public string Type { get; } = "MultiLineString";
    
    [JsonPropertyName("coordinates")]
    public List<List<List<double>>> Coordinates { get; set; } = new List<List<List<double>>>();
    
    [JsonPropertyName("bbox")]
    public List<double>? Bbox { get; set; }
}
#endregion

#region Property Types
public interface IProperties 
{
    string Type { get; }
}

public class WaypointProperties : IProperties
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Waypoint";
    
    [JsonPropertyName("routePathPoint")]
    public RoutePathPoint RoutePathPoint { get; set; } = new RoutePathPoint();
    
    [JsonPropertyName("order")]
    public Order Order { get; set; } = new Order();
}

public class RoutePathProperties : IProperties
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "RoutePath";
    
    [JsonPropertyName("resourceId")]
    public string ResourceId { get; set; }
    
    [JsonPropertyName("trafficDataUsed")]
    public string TrafficDataUsed { get; set; }
    
    [JsonPropertyName("distanceInMeters")]
    public double DistanceInMeters { get; set; }
    
    [JsonPropertyName("durationInSeconds")]
    public int DurationInSeconds { get; set; }
    
    [JsonPropertyName("departureAt")]
    public DateTime DepartureAt { get; set; }
    
    [JsonPropertyName("arrivalAt")]
    public DateTime ArrivalAt { get; set; }
    
    [JsonPropertyName("durationTrafficInSeconds")]
    public int DurationTrafficInSeconds { get; set; }
    
    [JsonPropertyName("legs")]
    public List<Leg> Legs { get; set; } = new List<Leg>();
    
    [JsonPropertyName("optimizedWaypoints")]
    public List<OptimizedWaypoint> OptimizedWaypoints { get; set; } = new List<OptimizedWaypoint>();
}

public class ManeuverPointProperties : IProperties
{
    [JsonPropertyName("durationInSeconds")]
    public int DurationInSeconds { get; set; }
    
    [JsonPropertyName("distanceInMeters")]
    public double DistanceInMeters { get; set; }
    
    [JsonPropertyName("routePathPoint")]
    public RoutePathPoint RoutePathPoint { get; set; }
    
    [JsonPropertyName("travelMode")]
    public string TravelMode { get; set; }
    
    [JsonPropertyName("instruction")]
    public Instruction Instruction { get; set; }
    
    [JsonPropertyName("address")]
    public Address Address { get; set; }
    
    [JsonPropertyName("towardsRoadName")]
    public string TowardsRoadName { get; set; }
    
    [JsonPropertyName("steps")]
    public List<Step> Steps { get; set; }
    
    [JsonPropertyName("order")]
    public Order Order { get; set; }

    public string Type { get; }
}

public class GenericProperties : IProperties
{
    public string? Type { get; set; }
    public string? RawJson { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}

#endregion

#region Supporting Classes
public class RoutePathPoint
{
    [JsonPropertyName("legIndex")]
    public int LegIndex { get; set; }
    
    [JsonPropertyName("pointIndex")]
    public int PointIndex { get; set; }
}

public class Order
{
    [JsonPropertyName("inputIndex")]
    public int InputIndex { get; set; }
}

public class Leg
{
    [JsonPropertyName("distanceInMeters")]
    public double DistanceInMeters { get; set; }
    
    [JsonPropertyName("durationInSeconds")]
    public int DurationInSeconds { get; set; }
    
    [JsonPropertyName("durationTrafficInSeconds")]
    public int DurationTrafficInSeconds { get; set; }
    
    [JsonPropertyName("departureAt")]
    public DateTime DepartureAt { get; set; }
    
    [JsonPropertyName("arrivalAt")]
    public DateTime ArrivalAt { get; set; }
    
    [JsonPropertyName("routePathRange")]
    public RoutePathRange RoutePathRange { get; set; } = new RoutePathRange();
}

public class RoutePathRange
{
    [JsonPropertyName("legIndex")]
    public int LegIndex { get; set; }
    
    [JsonPropertyName("range")]
    public List<int> Range { get; set; } = new List<int>();
}

public class OptimizedWaypoint
{
    [JsonPropertyName("inputIndex")]
    public int InputIndex { get; set; }
    
    [JsonPropertyName("optimizedIndex")]
    public int OptimizedIndex { get; set; }
}

public class Instruction
{
    [JsonPropertyName("formattedText")]
    public string FormattedText { get; set; }
    
    [JsonPropertyName("maneuverType")]
    public string ManeuverType { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
    
    [JsonPropertyName("drivingSide")]
    public string DrivingSide { get; set; }
}

public class Address
{
    [JsonPropertyName("countryRegion")]
    public CountryRegion CountryRegion { get; set; }
    
    [JsonPropertyName("adminDistricts")]
    public List<AdminDistrict> AdminDistricts { get; set; }
}

public class CountryRegion
{
    [JsonPropertyName("ISO")]
    public string ISO { get; set; }
}

public class AdminDistrict
{
    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }
}

public class Step
{
    [JsonPropertyName("maneuverType")]
    public string ManeuverType { get; set; }
    
    [JsonPropertyName("routePathRange")]
    public RoutePathRange RoutePathRange { get; set; }
    
    [JsonPropertyName("names")]
    public List<string> Names { get; set; }
}
#endregion