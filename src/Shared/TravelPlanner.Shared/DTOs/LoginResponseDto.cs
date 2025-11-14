namespace TravelPlanner.Shared.DTOs;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public DateTime Expiration { get; set; }
    public required string Username { get; set; }
}
