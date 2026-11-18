using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Edinici
{
    public int IdEd { get; set; }

    public string? NameEd { get; set; }

    public virtual ICollection<Tovary> Tovaries { get; set; } = new List<Tovary>();
}
