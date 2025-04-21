using System.Net;
using System.Text;
using System.Text.Json;
using AzureMapsRoutes.Exceptions;
using AzureMapsRoutes.Models;
using AzureMapsRoutes.Serializers;
using Microsoft.Extensions.Logging;

namespace AzureMapsRoutes;

public class AzureMapsRouteApiAccess
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _azureMapsApiEndpoint = "https://atlas.microsoft.com/";
    private readonly string _azureMapsSubscriptionKey;
    private readonly ILogger _logger;

    public AzureMapsRouteApiAccess(string azureMapsSubscriptionKey, ILogger logger)
    {
        _azureMapsSubscriptionKey = azureMapsSubscriptionKey;
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_azureMapsApiEndpoint)
        };
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = 
            {
                new FeatureConverter(),
                new GeometryJsonConverter(),
                new PropertiesJsonConverter()
            },
            WriteIndented = true
        };
    }
    
    public async Task<FeatureCollection> CalculateRoute(RouteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Beginning fetch from azure maps routes API, converting query to json string");
            var jsonRequest = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(jsonRequest, null, "application/json");

            _logger.LogInformation("Sending request");
            var response = await _httpClient.PostAsync("route/directions?api-version=2025-01-01&subscription-key=" + _azureMapsSubscriptionKey, content, cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation($"Received error message: {message}");
                throw new AzureMapsApiException(message);
            }
            
            _logger.LogInformation("Received success message, converting response to C# object");
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            
            _logger.LogInformation("Returning result");
            return JsonSerializer.Deserialize<FeatureCollection>(responseBody, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching route summary: {ex.Message}");
            throw;
        }
    }
}