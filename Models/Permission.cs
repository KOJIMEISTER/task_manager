using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();
}
