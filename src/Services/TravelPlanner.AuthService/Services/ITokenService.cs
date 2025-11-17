using TravelPlanner.AuthService.Models;

namespace TravelPlanner.AuthService.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
