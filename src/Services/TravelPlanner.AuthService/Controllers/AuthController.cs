using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.AuthService.Data;
using TravelPlanner.AuthService.DTOs;
using TravelPlanner.AuthService.Models;
using TravelPlanner.AuthService.Services;

namespace TravelPlanner.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(
        ApplicationDbContext context,
        ITokenService tokenService,
        ILogger<AuthController> logger,
        IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                // Check if user already exists
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    });
                }

                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Message = "Username already taken"
                    });
                }

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create new user
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    Role = "User", // Default role
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New user registered: {Email}", user.Email);

                return Ok(new RegisterResponseDto
                {
                    Success = true,
                    Message = "User registered successfully",
                    UserId = user.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new RegisterResponseDto
                {
                    Success = false,
                    Message = "An error occurred during registration"
                });
            }
        }

        /// <summary>
        /// Login and get JWT token
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginrRequestDto request)
        {
            try
            {
                // Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", request.Email);
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // Check if user is active
                if (!user.IsActive)
                {
                    return Unauthorized(new { message = "Account is deactivated" });
                }

                // Update last login time
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = _tokenService.GenerateJwtToken(user);
                var expiration = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["JwtSettings:ExpirationInMinutes"] ?? "1440")
                );

                _logger.LogInformation("User logged in successfully: {Email}", user.Email);

                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Expiration = expiration,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Get current user info (for testing token)
        /// </summary>
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                userId,
                username,
                email,
                role,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}
