using Microsoft.EntityFrameworkCore;
using TravelPlanner.FlightService.Data;
using TravelPlanner.FlightService.Models;

namespace TravelPlanner.FlightService.Tests;

public class FlightServiceTests
{
    private DbContextOptions<FlightDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<FlightDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetFlightRoutes_ReturnsAllRoutes()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        
        using (var context = new FlightDbContext(options))
        {
            context.FlightRoutes.Add(new FlightRoute
            {
                RouteId = "FL-001",
                From = "London Heathrow",
                To = "Paris CDG",
                Provider = "British Airways",
                Price = 89.99m,
                AvailableSeats = 120
            });
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new FlightDbContext(options))
        {
            var routes = await context.FlightRoutes.ToListAsync();

            // Assert
            Assert.Single(routes);
            Assert.Equal("FL-001", routes[0].RouteId);
        }
    }

    [Fact]
    public async Task AddFlightRoute_AddsSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        var newRoute = new FlightRoute
        {
            RouteId = "FL-002",
            From = "New York JFK",
            To = "Los Angeles LAX",
            Provider = "Delta Airlines",
            Price = 220m,
            AvailableSeats = 150
        };

        // Act
        using (var context = new FlightDbContext(options))
        {
            context.FlightRoutes.Add(newRoute);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new FlightDbContext(options))
        {
            var routes = await context.FlightRoutes.ToListAsync();
            Assert.Single(routes);
            Assert.Equal("FL-002", routes[0].RouteId);
        }
    }
}
