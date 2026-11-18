using System;
using System.Collections.Generic;

namespace api_floower.Models;

public partial class Status
{
    public int IdStatus { get; set; }

    public string? NameStatus { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
