using System.ComponentModel.DataAnnotations;

namespace task_management_system_api.Dtos;

public class UserCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [MaxLength(50)]
    public string? Firstname { get; set; }

    [MaxLength(50)]
    public string? Lastname { get; set; }
}