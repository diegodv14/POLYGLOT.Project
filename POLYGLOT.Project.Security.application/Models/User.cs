using System;
using System.Collections.Generic;

namespace POLYGLOT.Project.Security.application.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
