using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public string? NameCategory { get; set; }

    public virtual ICollection<Tovary> Tovaries { get; set; } = new List<Tovary>();
}
