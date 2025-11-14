using Microsoft.EntityFrameworkCore;
using TravelPlanner.BusService.Models;

namespace TravelPlanner.BusService.Data;

public class BusDbContext : DbContext
{
    public BusDbContext(DbContextOptions<BusDbContext> options) : base(options)
    {
    }

    public DbSet<BusRoute> BusRoutes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<BusRoute>().HasData(
            new BusRoute
            {
                Id = 1,
                RouteId = "BUS-315",
                From = "London Victoria Coach Station",
                To = "Paris Gallieni",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(8),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(16).AddMinutes(30),
                Duration = "8h 30m",
                Price = 35.00m,
                Currency = "EUR",
                Provider = "FlixBus",
                AvailableSeats = 28,
                Amenities = "WiFi,Power Outlets,Toilet"
            },
            new BusRoute
            {
                Id = 2,
                RouteId = "BUS-428",
                From = "Manchester",
                To = "Birmingham",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(9),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(11).AddMinutes(45),
                Duration = "2h 45m",
                Price = 15.00m,
                Currency = "GBP",
                Provider = "National Express",
                AvailableSeats = 35,
                Amenities = "WiFi,USB Charging"
            },
            new BusRoute
            {
                Id = 3,
                RouteId = "BUS-512",
                From = "New York Port Authority",
                To = "Boston South Station",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(10),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(14).AddMinutes(30),
                Duration = "4h 30m",
                Price = 25.00m,
                Currency = "USD",
                Provider = "Greyhound",
                AvailableSeats = 42,
                Amenities = "WiFi,Power Outlets,Toilet"
            }
        );
    }
}
