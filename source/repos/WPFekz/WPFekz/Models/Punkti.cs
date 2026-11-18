using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Punkti
{
    public int IdPunkti { get; set; }

    public string? Adress { get; set; }

    public virtual ICollection<Zakaz> Zakazs { get; set; } = new List<Zakaz>();
}
