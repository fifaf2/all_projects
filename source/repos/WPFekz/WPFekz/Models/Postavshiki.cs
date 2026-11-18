using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Postavshiki
{
    public int IdPost { get; set; }

    public string? NamePost { get; set; }

    public virtual ICollection<Tovary> Tovaries { get; set; } = new List<Tovary>();
}
