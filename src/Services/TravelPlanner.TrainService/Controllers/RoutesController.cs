using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.TrainService.Data;
using TravelPlanner.TrainService.Models;

namespace TravelPlanner.TrainService.Controllers;

[ApiController]
[Route("api/routes")]
[Authorize]
public class RoutesController : ControllerBase
{
    private readonly TrainDbContext _context;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(TrainDbContext context, ILogger<RoutesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainRoute>>> GetRoutes([FromQuery] string? from = null, [FromQuery] string? to = null)
    {
        _logger.LogInformation("Getting train routes. From: {From}, To: {To}", from, to);

        var query = _context.TrainRoutes.AsQueryable();

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
    public async Task<ActionResult<TrainRoute>> GetRoute(int id)
    {
        _logger.LogInformation("Getting train route with ID: {Id}", id);

        var route = await _context.TrainRoutes.FindAsync(id);

        if (route == null)
        {
            return NotFound(new { message = $"Train route with ID {id} not found." });
        }

        return Ok(route);
    }

    [HttpPost]
    public async Task<ActionResult<TrainRoute>> CreateRoute(TrainRoute route)
    {
        _logger.LogInformation("Creating new train route: {RouteId}", route.RouteId);

        _context.TrainRoutes.Add(route);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoute), new { id = route.Id }, route);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoute(int id, TrainRoute route)
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
            if (!await _context.TrainRoutes.AnyAsync(e => e.Id == id))
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
        var route = await _context.TrainRoutes.FindAsync(id);
        if (route == null)
        {
            return NotFound();
        }

        _context.TrainRoutes.Remove(route);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
