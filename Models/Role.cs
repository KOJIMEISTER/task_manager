using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public virtual ICollection<Projectuser> Projectusers { get; set; } = new List<Projectuser>();

    public virtual ICollection<Rolepermission> Rolepermissions { get; set; } = new List<Rolepermission>();

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
