using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Task
{
    public int Id { get; set; }

    public int Projectid { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? Duedate { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public int? Assigneeid { get; set; }

    public int Createdby { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public virtual User? Assignee { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<Taskcomment> Taskcomments { get; set; } = new List<Taskcomment>();
}
