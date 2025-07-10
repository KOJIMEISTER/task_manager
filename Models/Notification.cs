using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Notification
{
    public Guid Id { get; set; }

    public Guid Userid { get; set; }

    public string Content { get; set; } = null!;

    public bool Isread { get; set; }

    public DateTime Createdat { get; set; }

    public virtual User User { get; set; } = null!;
}
