using task_management_system_api.Models;

namespace task_management_system_api.Dtos;
public class UserRoleReadDto
{
    public Guid Userid { get; set; }

    public Guid Roleid { get; set; }

    public DateTime Assignedat { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}