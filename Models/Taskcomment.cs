using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Taskcomment
{
    public Guid Id { get; set; }

    public Guid Taskid { get; set; }

    public Guid Userid { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
