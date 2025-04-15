﻿using System;
using System.Collections.Generic;

namespace task_management_system_api.Models;

public partial class Userrole
{
    public int Userid { get; set; }

    public int Roleid { get; set; }

    public DateTime Assignedat { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
