using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management_system_api.Dtos;
using task_management_system_api.Models;
using task_management_system_api.Utils;

namespace task_management_system_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly IPasswordService _passwordService;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    public UserController(AppDbContext context, IMapper mapper, ILogger<UserController> logger, IPasswordService passwordService)
    {
        _passwordService = passwordService;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/users
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        _logger.LogInformation("Fetching users: number-{PageNumber}, size-{PageSize}", pageNumber, pageSize);
        var users = await _context.Users
        .AsNoTracking()
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        var usersDto = _mapper.Map<IEnumerable<UserReadDto>>(users);
        return Ok(usersDto);
    }

    // GET api/v1/users/{id}
    [Authorize]
    [HttpGet("{id:guid}", Name = "GetUserById")]
    public async Task<ActionResult<UserReadDto>> GetUserById(Guid id)
    {
        _logger.LogInformation("Fetching user: id-{id}", id);
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogError("Fetching error: user {id} not found!", id);
            return NotFound(new { Message = "User not found" });
        }
        var userDto = _mapper.Map<UserReadDto>(user);
        return Ok(userDto);
    }

    // POST api/v1/users
    [Authorize(Policy = "Admin")] // more roles?
    [HttpPost]
    public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userDto)
    {
        _logger.LogInformation("Create user: {username}", userDto.Username);
        var user = _mapper.Map<User>(userDto);
        user.Id = Guid.NewGuid();
        user.Createdat = DateTime.UtcNow;
        byte[] salt = _passwordService.GenerateSalt();
        byte[] hashedPassword = await _passwordService.HashPasswordAsync(userDto.Password, salt);
        user.Passwordsalt = System.Text.Encoding.UTF8.GetString(salt);
        user.Passwordhash = System.Text.Encoding.UTF8.GetString(hashedPassword);
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for CreateUser!");
            return BadRequest(ModelState);
        }
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userReadDto = _mapper.Map<UserReadDto>(user);
        return CreatedAtRoute("GetUserById", new { id = user.Id, version = "1.0" }, userReadDto);
    }

    // PUT api/v1/users/{id}
    [Authorize(Policy = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateUser(Guid id, UserUpdateDto userUpdateDto)
    {
        _logger.LogInformation("Start updating user {id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogError("Invalid model state for UpdateUser");
            return BadRequest(ModelState);
        }
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogError("User with id: {id} not found", id);
            return NotFound(new { Message = "User not found" });
        }
        _mapper.Map(userUpdateDto, user);
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("User with id: {id} updated successfully", id);
        }
        catch (DbUpdateConcurrencyException)
        {
            _logger.LogError("Concurrency error when updating user with id: {id}", id);
            return StatusCode(500, new { Message = "A concurrency error occured" });
        }
        return NoContent();
    }

    // DELETE api/v1/users/{id}
    [Authorize(Policy = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        _logger.LogInformation("Deleting user with id: {id}", id);
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogError("User with id: {id} not found", id);
            return NotFound(new { Message = "User not found" });
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User with id: {id} successfully deleted", id);
        return NoContent();
    }

    // PUT api/v1/users/change-password
    [Authorize]
    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePassword()
    {
        return NoContent();
    }
}