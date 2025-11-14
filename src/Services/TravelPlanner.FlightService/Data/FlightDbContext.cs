using Microsoft.EntityFrameworkCore;
using TravelPlanner.FlightService.Models;

namespace TravelPlanner.FlightService.Data;

public class FlightDbContext : DbContext
{
    public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
    {
    }

    public DbSet<FlightRoute> FlightRoutes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<FlightRoute>().HasData(
            new FlightRoute
            {
                Id = 1,
                RouteId = "FL-042",
                From = "London Heathrow (LHR)",
                To = "Paris Charles de Gaulle (CDG)",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(10).AddMinutes(15),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(12).AddMinutes(30),
                Duration = "1h 15m",
                Price = 89.99m,
                Currency = "EUR",
                Provider = "British Airways",
                AvailableSeats = 120,
                Class = "Economy",
                FlightNumber = "BA308",
                Amenities = "In-flight Entertainment,Meals,WiFi"
            },
            new FlightRoute
            {
                Id = 2,
                RouteId = "FL-187",
                From = "Manchester Airport (MAN)",
                To = "Dubai International (DXB)",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(14),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(23).AddMinutes(30),
                Duration = "6h 30m",
                Price = 450.00m,
                Currency = "GBP",
                Provider = "Emirates",
                AvailableSeats = 85,
                Class = "Business",
                FlightNumber = "EK018",
                Amenities = "Lie-flat Seats,Gourmet Meals,Premium Entertainment,WiFi"
            },
            new FlightRoute
            {
                Id = 3,
                RouteId = "FL-293",
                From = "New York JFK (JFK)",
                To = "Los Angeles (LAX)",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(8),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(14).AddMinutes(30),
                Duration = "5h 30m",
                Price = 220.00m,
                Currency = "USD",
                Provider = "Delta Airlines",
                AvailableSeats = 150,
                Class = "Economy",
                FlightNumber = "DL302",
                Amenities = "In-flight Entertainment,Snacks,WiFi"
            }
        );
    }
}
