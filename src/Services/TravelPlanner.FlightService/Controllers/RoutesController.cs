using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.FlightService.Data;
using TravelPlanner.FlightService.Models;

namespace TravelPlanner.FlightService.Controllers;

[ApiController]
[Route("api/routes")]
[Authorize]
public class RoutesController : ControllerBase
{
    private readonly FlightDbContext _context;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(FlightDbContext context, ILogger<RoutesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlightRoute>>> GetRoutes([FromQuery] string? from = null, [FromQuery] string? to = null)
    {
        _logger.LogInformation("Getting flight routes. From: {From}, To: {To}", from, to);

        var query = _context.FlightRoutes.AsQueryable();

        if (!string.IsNullOrEmpty(from))
        {
            query = query.Where(r => r.From.Contains(from));
        }

        if (!string.IsNullOrEmpty(to))
        {
            query = query.Where(r => r.To.Contains(to));
        }

        var routes = await query.ToListAsync();
        return Ok(routes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FlightRoute>> GetRoute(int id)
    {
        _logger.LogInformation("Getting flight route with ID: {Id}", id);

        var route = await _context.FlightRoutes.FindAsync(id);

        if (route == null)
        {
            return NotFound(new { message = $"Flight route with ID {id} not found." });
        }

        return Ok(route);
    }

    [HttpPost]
    public async Task<ActionResult<FlightRoute>> CreateRoute(FlightRoute route)
    {
        _logger.LogInformation("Creating new flight route: {RouteId}", route.RouteId);

        _context.FlightRoutes.Add(route);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoute), new { id = route.Id }, route);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoute(int id, FlightRoute route)
    {
        if (id != route.Id)
        {
            return BadRequest(new { message = "Route ID mismatch." });
        }

        _context.Entry(route).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.FlightRoutes.AnyAsync(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoute(int id)
    {
        var route = await _context.FlightRoutes.FindAsync(id);
        if (route == null)
        {
            return NotFound();
        }

        _context.FlightRoutes.Remove(route);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
