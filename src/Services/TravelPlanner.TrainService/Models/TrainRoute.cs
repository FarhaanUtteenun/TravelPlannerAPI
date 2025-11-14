namespace TravelPlanner.TrainService.Models;

public class TrainRoute
{
    public int Id { get; set; }
    public required string RouteId { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Duration { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public required string Provider { get; set; }
    public int AvailableSeats { get; set; }
    public string Class { get; set; } = "Standard";
    public string? Amenities { get; set; }
}
