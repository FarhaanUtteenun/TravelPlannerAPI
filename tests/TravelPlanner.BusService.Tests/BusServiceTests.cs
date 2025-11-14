using Microsoft.EntityFrameworkCore;
using TravelPlanner.BusService.Data;
using TravelPlanner.BusService.Models;

namespace TravelPlanner.BusService.Tests;

public class BusServiceTests
{
    private DbContextOptions<BusDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<BusDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetBusRoutes_ReturnsAllRoutes()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        
        using (var context = new BusDbContext(options))
        {
            context.BusRoutes.Add(new BusRoute
            {
                RouteId = "BUS-001",
                From = "London",
                To = "Paris",
                Provider = "FlixBus",
                Price = 35m,
                AvailableSeats = 28
            });
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new BusDbContext(options))
        {
            var routes = await context.BusRoutes.ToListAsync();

            // Assert
            Assert.Single(routes);
            Assert.Equal("BUS-001", routes[0].RouteId);
        }
    }

    [Fact]
    public async Task AddBusRoute_AddsSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        var newRoute = new BusRoute
        {
            RouteId = "BUS-002",
            From = "Manchester",
            To = "Birmingham",
            Provider = "National Express",
            Price = 15m,
            AvailableSeats = 35
        };

        // Act
        using (var context = new BusDbContext(options))
        {
            context.BusRoutes.Add(newRoute);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new BusDbContext(options))
        {
            var routes = await context.BusRoutes.ToListAsync();
            Assert.Single(routes);
            Assert.Equal("BUS-002", routes[0].RouteId);
        }
    }
}
