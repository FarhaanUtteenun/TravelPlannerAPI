using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelPlanner.TrainService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainRoutes",
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
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainRoutes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainRoutes",
                columns: new[] { "Id", "Amenities", "ArrivalTime", "AvailableSeats", "Class", "Currency", "DepartureTime", "Duration", "From", "Price", "Provider", "RouteId", "To" },
                values: new object[,]
                {
                    { 1, "WiFi,Power Outlets,Refreshments", new DateTime(2025, 11, 17, 8, 57, 38, 279, DateTimeKind.Local).AddTicks(2050), 45, "Standard", "EUR", new DateTime(2025, 11, 17, 6, 27, 38, 279, DateTimeKind.Local).AddTicks(2027), "2h 30m", "London St Pancras", 120.50m, "Eurostar", "TR-001", "Paris Gare du Nord" },
                    { 2, "WiFi,Power Outlets,Refreshments", new DateTime(2025, 11, 17, 8, 42, 38, 279, DateTimeKind.Local).AddTicks(2060), 67, "Standard", "GBP", new DateTime(2025, 11, 17, 4, 57, 38, 279, DateTimeKind.Local).AddTicks(2059), "3h 45m", "Manchester Piccadilly", 85.00m, "Avanti West Coast", "TR-156", "Edinburgh Waverley" },
                    { 3, "WiFi,Power Outlets,Cafe Car", new DateTime(2025, 11, 17, 10, 42, 38, 279, DateTimeKind.Local).AddTicks(2065), 120, "Business", "USD", new DateTime(2025, 11, 17, 7, 27, 38, 279, DateTimeKind.Local).AddTicks(2063), "3h 15m", "New York Penn Station", 98.00m, "Amtrak", "TR-203", "Washington DC Union Station" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainRoutes");
        }
    }
}
