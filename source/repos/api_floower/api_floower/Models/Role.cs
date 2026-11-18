using System;
using System.Collections.Generic;

namespace api_floower.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string? NameRole { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
