namespace TravelPlanner.Shared.DTOs;

public class SearchCriteriaDto
{
    public required string From { get; set; }
    public required string To { get; set; }
    public DateTime? DepartureDate { get; set; }
    public int? MaxResults { get; set; }
}
