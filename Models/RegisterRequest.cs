using System.ComponentModel.DataAnnotations;

namespace task_management_system_api.Models;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }
}