using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Manufacture
{
    public int IdManufac { get; set; }

    public string? NameManufac { get; set; }

    public virtual ICollection<Tovary> Tovaries { get; set; } = new List<Tovary>();
}
