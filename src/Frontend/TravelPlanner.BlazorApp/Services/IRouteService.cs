namespace TravelPlanner.BlazorApp.Services;

public interface IRouteService
{
    Task<IEnumerable<RouteDto>> GetRoutesAsync(string from, string to);
}

public class RouteDto
{
    public string RouteId { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Duration { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string Provider { get; set; } = string.Empty;
    public int AvailableSeats { get; set; }
}
