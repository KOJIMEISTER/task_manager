namespace task_management_system_api.Models;

public class AuthResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Username { get; set; }
}