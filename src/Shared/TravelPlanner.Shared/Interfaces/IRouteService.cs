namespace TravelPlanner.Shared.Interfaces;

public interface IRouteService
{
    Task<IEnumerable<object>> GetRoutesAsync(string? from = null, string? to = null);
    Task<object?> GetRouteByIdAsync(int id);
}
