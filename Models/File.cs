using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class File
{
    public int Id { get; set; }

    public string Filename { get; set; } = null!;

    public string Filepath { get; set; } = null!;

    public int Uploadedby { get; set; }

    public DateTime Uploadedat { get; set; }

    public int? Projectid { get; set; }

    public int? Taskid { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Task? Task { get; set; }

    public virtual User UploadedbyNavigation { get; set; } = null!;
}
