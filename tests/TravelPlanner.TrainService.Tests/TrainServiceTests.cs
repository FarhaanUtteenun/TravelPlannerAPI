using Microsoft.EntityFrameworkCore;
using TravelPlanner.TrainService.Data;
using TravelPlanner.TrainService.Models;

namespace TravelPlanner.TrainService.Tests;

public class TrainServiceTests
{
    private DbContextOptions<TrainDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<TrainDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetTrainRoutes_ReturnsAllRoutes()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        
        using (var context = new TrainDbContext(options))
        {
            context.TrainRoutes.Add(new TrainRoute
            {
                RouteId = "TEST-001",
                From = "London",
                To = "Paris",
                Provider = "Test Provider",
                Price = 100m,
                AvailableSeats = 50
            });
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new TrainDbContext(options))
        {
            var routes = await context.TrainRoutes.ToListAsync();

            // Assert
            Assert.Single(routes);
            Assert.Equal("TEST-001", routes[0].RouteId);
        }
    }

    [Fact]
    public async Task AddTrainRoute_AddsSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        var newRoute = new TrainRoute
        {
            RouteId = "TEST-002",
            From = "Manchester",
            To = "Edinburgh",
            Provider = "Test Provider",
            Price = 85m,
            AvailableSeats = 60
        };

        // Act
        using (var context = new TrainDbContext(options))
        {
            context.TrainRoutes.Add(newRoute);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new TrainDbContext(options))
        {
            var routes = await context.TrainRoutes.ToListAsync();
            Assert.Single(routes);
            Assert.Equal("TEST-002", routes[0].RouteId);
        }
    }
}
