using Microsoft.EntityFrameworkCore;
using TravelPlanner.TrainService.Models;

namespace TravelPlanner.TrainService.Data;

public class TrainDbContext : DbContext
{
    public TrainDbContext(DbContextOptions<TrainDbContext> options) : base(options)
    {
    }

    public DbSet<TrainRoute> TrainRoutes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<TrainRoute>().HasData(
            new TrainRoute
            {
                Id = 1,
                RouteId = "TR-001",
                From = "London St Pancras",
                To = "Paris Gare du Nord",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(9),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(11).AddMinutes(30),
                Duration = "2h 30m",
                Price = 120.50m,
                Currency = "EUR",
                Provider = "Eurostar",
                AvailableSeats = 45,
                Class = "Standard",
                Amenities = "WiFi,Power Outlets,Refreshments"
            },
            new TrainRoute
            {
                Id = 2,
                RouteId = "TR-156",
                From = "Manchester Piccadilly",
                To = "Edinburgh Waverley",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(7).AddMinutes(30),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(11).AddMinutes(15),
                Duration = "3h 45m",
                Price = 85.00m,
                Currency = "GBP",
                Provider = "Avanti West Coast",
                AvailableSeats = 67,
                Class = "Standard",
                Amenities = "WiFi,Power Outlets,Refreshments"
            },
            new TrainRoute
            {
                Id = 3,
                RouteId = "TR-203",
                From = "New York Penn Station",
                To = "Washington DC Union Station",
                DepartureTime = DateTime.Now.AddDays(1).AddHours(10),
                ArrivalTime = DateTime.Now.AddDays(1).AddHours(13).AddMinutes(15),
                Duration = "3h 15m",
                Price = 98.00m,
                Currency = "USD",
                Provider = "Amtrak",
                AvailableSeats = 120,
                Class = "Business",
                Amenities = "WiFi,Power Outlets,Cafe Car"
            }
        );
    }
}
