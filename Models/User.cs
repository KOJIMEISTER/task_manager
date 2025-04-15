using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public bool Isactive { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Projectuser> Projectusers { get; set; } = new List<Projectuser>();

    public virtual ICollection<Refreshtoken> Refreshtokens { get; set; } = new List<Refreshtoken>();

    public virtual ICollection<Task> TaskAssignees { get; set; } = new List<Task>();

    public virtual ICollection<Task> TaskCreatedbyNavigations { get; set; } = new List<Task>();

    public virtual ICollection<Taskcomment> Taskcomments { get; set; } = new List<Taskcomment>();

    public virtual ICollection<Userrole> Userroles { get; set; } = new List<Userrole>();
}
