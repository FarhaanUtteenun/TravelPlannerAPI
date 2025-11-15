using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelPlanner.BusService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BusRoutes",
                columns: new[] { "Id", "Amenities", "ArrivalTime", "AvailableSeats", "Currency", "DepartureTime", "Duration", "From", "Price", "Provider", "RouteId", "To" },
                values: new object[,]
                {
                    { 1, "WiFi,Power Outlets,Toilet", new DateTime(2025, 11, 17, 13, 51, 36, 378, DateTimeKind.Local).AddTicks(850), 28, "EUR", new DateTime(2025, 11, 17, 5, 21, 36, 378, DateTimeKind.Local).AddTicks(832), "8h 30m", "London Victoria Coach Station", 35.00m, "FlixBus", "BUS-315", "Paris Gallieni" },
                    { 2, "WiFi,USB Charging", new DateTime(2025, 11, 17, 9, 6, 36, 378, DateTimeKind.Local).AddTicks(858), 35, "GBP", new DateTime(2025, 11, 17, 6, 21, 36, 378, DateTimeKind.Local).AddTicks(857), "2h 45m", "Manchester", 15.00m, "National Express", "BUS-428", "Birmingham" },
                    { 3, "WiFi,Power Outlets,Toilet", new DateTime(2025, 11, 17, 11, 51, 36, 378, DateTimeKind.Local).AddTicks(862), 42, "USD", new DateTime(2025, 11, 17, 7, 21, 36, 378, DateTimeKind.Local).AddTicks(861), "4h 30m", "New York Port Authority", 25.00m, "Greyhound", "BUS-512", "Boston South Station" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusRoutes");
        }
    }
}
