namespace task_management_system_api.Models;

public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}