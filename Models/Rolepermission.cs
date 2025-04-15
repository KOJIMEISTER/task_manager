using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Rolepermission
{
    public int Roleid { get; set; }

    public int Permissionid { get; set; }

    public DateTime Assignedat { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
