using System.ComponentModel.DataAnnotations;

namespace task_management_system_api.Models;
public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}