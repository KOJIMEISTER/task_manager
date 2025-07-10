using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Projectuser
{
    public Guid Projectid { get; set; }

    public Guid Userid { get; set; }

    public Guid Roleid { get; set; }

    public DateTime Assignedat { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
