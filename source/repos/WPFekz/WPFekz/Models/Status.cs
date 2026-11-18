using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Status
{
    public int IdStatus { get; set; }

    public string? NameStatus { get; set; }

    public virtual ICollection<Zakaz> Zakazs { get; set; } = new List<Zakaz>();
}
