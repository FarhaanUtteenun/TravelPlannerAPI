using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelPlanner.FlightService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightRoutes",
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
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightRoutes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FlightRoutes",
                columns: new[] { "Id", "Amenities", "ArrivalTime", "AvailableSeats", "Class", "Currency", "DepartureTime", "Duration", "FlightNumber", "From", "Price", "Provider", "RouteId", "To" },
                values: new object[,]
                {
                    { 1, "In-flight Entertainment,Meals,WiFi", new DateTime(2025, 11, 17, 9, 54, 41, 735, DateTimeKind.Local).AddTicks(217), 120, "Economy", "EUR", new DateTime(2025, 11, 17, 7, 39, 41, 735, DateTimeKind.Local).AddTicks(188), "1h 15m", "BA308", "London Heathrow (LHR)", 89.99m, "British Airways", "FL-042", "Paris Charles de Gaulle (CDG)" },
                    { 2, "Lie-flat Seats,Gourmet Meals,Premium Entertainment,WiFi", new DateTime(2025, 11, 17, 20, 54, 41, 735, DateTimeKind.Local).AddTicks(228), 85, "Business", "GBP", new DateTime(2025, 11, 17, 11, 24, 41, 735, DateTimeKind.Local).AddTicks(227), "6h 30m", "EK018", "Manchester Airport (MAN)", 450.00m, "Emirates", "FL-187", "Dubai International (DXB)" },
                    { 3, "In-flight Entertainment,Snacks,WiFi", new DateTime(2025, 11, 17, 11, 54, 41, 735, DateTimeKind.Local).AddTicks(235), 150, "Economy", "USD", new DateTime(2025, 11, 17, 5, 24, 41, 735, DateTimeKind.Local).AddTicks(233), "5h 30m", "DL302", "New York JFK (JFK)", 220.00m, "Delta Airlines", "FL-293", "Los Angeles (LAX)" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightRoutes");
        }
    }
}
