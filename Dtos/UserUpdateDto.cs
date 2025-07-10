using System.ComponentModel.DataAnnotations;

namespace task_management_system_api.Dtos;

public class UserUpdateDto
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Password { get; set; }

    [MaxLength(50)]
    public string? Firstname { get; set; }

    [MaxLength(50)]
    public string? Lastname { get; set; }

    public bool? Isactive { get; set; }
}