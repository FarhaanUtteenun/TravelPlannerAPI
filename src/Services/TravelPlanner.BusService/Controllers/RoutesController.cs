using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.BusService.Data;
using TravelPlanner.BusService.Models;

namespace TravelPlanner.BusService.Controllers;

[ApiController]
[Route("api/routes")]
[Authorize]
public class RoutesController : ControllerBase
{
    private readonly BusDbContext _context;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(BusDbContext context, ILogger<RoutesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BusRoute>>> GetRoutes([FromQuery] string? from = null, [FromQuery] string? to = null)
    {
        _logger.LogInformation("Getting bus routes. From: {From}, To: {To}", from, to);

        var query = _context.BusRoutes.AsQueryable();

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
    public async Task<ActionResult<BusRoute>> GetRoute(int id)
    {
        _logger.LogInformation("Getting bus route with ID: {Id}", id);

        var route = await _context.BusRoutes.FindAsync(id);

        if (route == null)
        {
            return NotFound(new { message = $"Bus route with ID {id} not found." });
        }

        return Ok(route);
    }

    [HttpPost]
    public async Task<ActionResult<BusRoute>> CreateRoute(BusRoute route)
    {
        _logger.LogInformation("Creating new bus route: {RouteId}", route.RouteId);

        _context.BusRoutes.Add(route);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoute), new { id = route.Id }, route);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoute(int id, BusRoute route)
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
            if (!await _context.BusRoutes.AnyAsync(e => e.Id == id))
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
        var route = await _context.BusRoutes.FindAsync(id);
        if (route == null)
        {
            return NotFound();
        }

        _context.BusRoutes.Remove(route);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
