using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TravelPlanner.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoutesController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(
        IHttpClientFactory httpClientFactory,
        ILogger<RoutesController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Get routes from ALL services (train, bus, flight) in parallel
    /// Gateway aggregates results from all three microservices
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllRoutes(
        [FromQuery] string? from,
        [FromQuery] string? to)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Forward JWT token from incoming request
            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", authHeader.ToString());
            }

            // Build query string
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(from))
                queryParams.Add($"from={Uri.EscapeDataString(from)}");
            if (!string.IsNullOrEmpty(to))
                queryParams.Add($"to={Uri.EscapeDataString(to)}");

            var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";

            _logger.LogInformation(
                "Aggregating routes from all services for query: {Query}",
                queryString
            );

            // Call all three services in PARALLEL
            var trainTask = CallServiceAsync(
                httpClient,
                $"https://localhost:49892/api/routes{queryString}",
                "Train"
            );

            var busTask = CallServiceAsync(
                httpClient,
                $"https://localhost:49893/api/routes{queryString}",
                "Bus"
            );

            var flightTask = CallServiceAsync(
                httpClient,
                $"https://localhost:49889/api/routes{queryString}",
                "Flight"
            );

            // Wait for all to complete
            await Task.WhenAll(trainTask, busTask, flightTask);

            var trainResult = await trainTask;
            var busResult = await busTask;
            var flightResult = await flightTask;

            var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

            // Combine all results
            var response = new
            {
                searchCriteria = new
                {
                    from,
                    to,
                    timestamp = DateTime.UtcNow
                },
                results = new
                {
                    train = new
                    {
                        success = trainResult.Success,
                        count = trainResult.Data?.Count ?? 0,
                        routes = trainResult.Data,
                        error = trainResult.Error
                    },
                    bus = new
                    {
                        success = busResult.Success,
                        count = busResult.Data?.Count ?? 0,
                        routes = busResult.Data,
                        error = busResult.Error
                    },
                    flight = new
                    {
                        success = flightResult.Success,
                        count = flightResult.Data?.Count ?? 0,
                        routes = flightResult.Data,
                        error = flightResult.Error
                    }
                },
                summary = new
                {
                    totalRoutes = (trainResult.Data?.Count ?? 0) +
                                  (busResult.Data?.Count ?? 0) +
                                  (flightResult.Data?.Count ?? 0),
                    servicesQueried = 3,
                    servicesSuccessful = new[] { trainResult.Success, busResult.Success, flightResult.Success }
                        .Count(x => x),
                    executionTimeMs = executionTime
                }
            };

            _logger.LogInformation(
                "Route aggregation completed in {Ms}ms. Total routes: {Count}",
                executionTime,
                response.summary.totalRoutes
            );

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aggregating routes from services");
            return StatusCode(500, new
            {
                error = "Failed to retrieve routes",
                details = ex.Message
            });
        }
    }

    /// <summary>
    /// Get route by ID from specific service
    /// </summary>
    [HttpGet("{serviceType}/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRouteById(string serviceType, int id)
    {
        var httpClient = _httpClientFactory.CreateClient();

        if (Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", authHeader.ToString());
        }

        var port = serviceType.ToLower() switch
        {
            "train" => 49892,
            "bus" => 49893,
            "flight" => 49889,
            _ => 0
        };

        if (port == 0)
        {
            return BadRequest(new { error = $"Invalid service type: {serviceType}. Valid types: train, bus, flight" });
        }

        try
        {
            var response = await httpClient.GetAsync($"https://localhost:{port}/api/routes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var route = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return Ok(route);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { error = $"Route with ID {id} not found in {serviceType} service" });
            }

            return StatusCode((int)response.StatusCode, new { error = $"Error from {serviceType} service" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving route {Id} from {Service}", id, serviceType);
            return StatusCode(500, new { error = "Failed to retrieve route" });
        }
    }

    private async Task<ServiceResult> CallServiceAsync(HttpClient client, string url, string serviceName)
    {
        try
        {
            _logger.LogDebug("Calling {Service} service at {Url}", serviceName, url);

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var routes = JsonSerializer.Deserialize<List<object>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogDebug(
                    "{Service} service returned {Count} routes with status {Status}",
                    serviceName,
                    routes?.Count ?? 0,
                    response.StatusCode
                );

                return new ServiceResult
                {
                    Success = true,
                    Data = routes ?? new List<object>()
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning(
                    "{Service} service returned {Status}: {Error}",
                    serviceName,
                    response.StatusCode,
                    errorContent
                );

                return new ServiceResult
                {
                    Success = false,
                    Error = $"{serviceName} service returned {response.StatusCode}"
                };
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "{Service} service is unreachable: {Message}", serviceName, ex.Message);
            return new ServiceResult
            {
                Success = false,
                Error = $"{serviceName} service is unavailable"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling {Service} service: {Message}", serviceName, ex.Message);
            return new ServiceResult
            {
                Success = false,
                Error = $"Error calling {serviceName}: {ex.Message}"
            };
        }
    }

    private class ServiceResult
    {
        public bool Success { get; set; }
        public List<object>? Data { get; set; }
        public string? Error { get; set; }
    }
}
