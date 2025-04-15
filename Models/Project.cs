using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly Startdate { get; set; }

    public DateOnly? Enddate { get; set; }

    public int Createdby { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Projectuser> Projectusers { get; set; } = new List<Projectuser>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
