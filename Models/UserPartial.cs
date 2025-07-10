namespace task_management_system_api.Models;
public partial class Refreshtoken
{
    public bool IsExpired => DateTime.UtcNow >= Expiresat;
}