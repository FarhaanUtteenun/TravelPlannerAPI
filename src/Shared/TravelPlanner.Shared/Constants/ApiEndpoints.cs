namespace TravelPlanner.Shared.Constants;

public static class ApiEndpoints
{
    public const string Gateway = "https://localhost:5000";
    public const string TrainService = "https://localhost:5100";
    public const string BusService = "https://localhost:5200";
    public const string FlightService = "https://localhost:5300";
    
    public static class Routes
    {
        public const string GetAll = "/api/routes";
        public const string GetById = "/api/routes/{0}";
        public const string Search = "/api/routes?from={0}&to={1}";
    }
    
    public static class Auth
    {
        public const string Login = "/api/auth/login";
        public const string Register = "/api/auth/register";
    }
}
