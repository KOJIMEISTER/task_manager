using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using task_management_system_api.Models;
using task_management_system_api.Utils;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;
    private readonly IPasswordService _passwordService;
    private readonly JwtSettings _jwtSettings;
    public AuthController(AppDbContext context, IMapper mapper, ILogger<AuthController> logger,
    IOptions<JwtSettings> jwtSettings, IPasswordService passwordService)
    {
        _passwordService = passwordService;
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _jwtSettings = jwtSettings.Value;
    }

    // POST api/v1/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("User with username: {username} trying to login", loginRequest.Username);
        var user = await _context.Users.Include(user => user.Refreshtokens).FirstOrDefaultAsync(user => user.Username == loginRequest.Username);
        if (user == null)
        {
            _logger.LogError("User with username: {username}", loginRequest.Username);
            return Unauthorized(new { Message = "Invalid credentials" });
        }
        bool validPass = await _passwordService.VerifyPasswordAsync(loginRequest.Password,
        Convert.FromBase64String(user.Passwordhash),
        Convert.FromBase64String(user.Passwordsalt));
        if (!validPass)
        {
            _logger.LogError("User with username: {username} send wrong password", loginRequest.Username);
            return Unauthorized(new { Message = "Invalid credentials" });
        }
        var response = MakeAuthorize(user);
        _logger.LogInformation("User with username: {username} successfully logged", loginRequest.Username);
        return Ok(response);
    }
    private async Task<AuthResponse> MakeAuthorize(User user)
    {
        user.Isactive = true;
        var token = GenerateToken(user);
        var refreshToken = GenerateRefreshToken(user);
        _context.Refreshtokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        var response = new AuthResponse
        {
            Username = user.Username,
            Token = token,
            RefreshToken = refreshToken.Token
        };
        return response;
    }
    private string GenerateToken(User user)
    {
        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            signingCredentials: creds,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Refreshtoken GenerateRefreshToken(User user)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return new Refreshtoken
        {
            Id = new Guid(),
            Userid = user.Id,
            Token = Convert.ToBase64String(randomBytes),
            Expiresat = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            Createdat = DateTime.UtcNow,
            User = user
        };
    }

    // POST api/v1/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest registerRequest)
    {
        _logger.LogInformation("Registering user: {username}", registerRequest.Username);
        if (!ModelState.IsValid)
        {
            BadRequest();
        }
        var isUserExists = await _context.Users.AnyAsync(u => u.Username == registerRequest.Username);
        if (isUserExists)
        {
            _logger.LogWarning("Registration failed. Username {username} is already taken.", registerRequest.Username);
            return Conflict(new { Message = "Username is already taken." });
        }
        var hashedSalt = _passwordService.GenerateSalt();
        var hashedPassword = await _passwordService.HashPasswordAsync(registerRequest.Password, hashedSalt);
        var user = new User
        {
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            Passwordhash = Convert.ToBase64String(hashedPassword),
            Passwordsalt = Convert.ToBase64String(hashedSalt),
            Createdat = DateTime.UtcNow,
            Updatedat = DateTime.UtcNow,
            Isactive = true
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User {username} registered successfully.", registerRequest.Username);
        var response = MakeAuthorize(user);
        return CreatedAtAction(nameof(Register), response);
    }

    // POST api/v1/auth/refresh-token
    [HttpPost("refresh-token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        _logger.LogInformation("Attempting to refresh token");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingRefreshToken = await _context.Refreshtokens.Include(rt => rt.User)
        .FirstOrDefaultAsync(rt => rt.Token == refreshTokenRequest.RefreshToken);
        if (existingRefreshToken == null || existingRefreshToken.IsExpired)
        {
            _logger.LogWarning("Invalid or expired refresh token.");
            return Unauthorized(new { Message = "Invalid or expired refresh token." });
        }
        _context.Refreshtokens.Remove(existingRefreshToken);
        var token = GenerateToken(existingRefreshToken.User);
        var refreshToken = GenerateRefreshToken(existingRefreshToken.User);
        _context.Refreshtokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        var response = new RefreshTokenResponse
        {
            Token = token,
            RefreshToken = refreshToken.Token
        };
        return Ok(response);
    }

    //POST api/v1/auth/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        var username = User.Identity.Name;
        _logger.LogInformation("User {username} is logging out.", username);
        var user = await _context.Users.Include(u => u.Refreshtokens).FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            _logger.LogWarning("Logout failed. User {username} not found.", username);
            return NotFound(new { Message = "User not found." });
        }
        foreach (var refreshToken in user.Refreshtokens)
        {
            _context.Refreshtokens.Remove(refreshToken);
        }
        user.Isactive = false;
        await _context.SaveChangesAsync();
        _logger.LogInformation("User {username} logged out successfully.", username);
        return Ok(new { Message = "Logged out successfully." });
    }
}