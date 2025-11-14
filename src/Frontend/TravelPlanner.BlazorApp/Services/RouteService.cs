using System.Net.Http.Json;

namespace TravelPlanner.BlazorApp.Services;

public class RouteService : IRouteService
{
    private readonly HttpClient _httpClient;

    public RouteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<RouteDto>> GetRoutesAsync(string from, string to)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<RouteDto>>($"/api/routes?from={from}&to={to}");
            return response ?? Enumerable.Empty<RouteDto>();
        }
        catch
        {
            return Enumerable.Empty<RouteDto>();
        }
    }
}
