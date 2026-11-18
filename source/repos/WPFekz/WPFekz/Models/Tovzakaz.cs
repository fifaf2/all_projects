using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Tovzakaz
{
    public int IdTovZakaz { get; set; }

    public int? IdZakaz { get; set; }

    public int? Id { get; set; }

    public int? Order { get; set; }

    public virtual Tovary? IdNavigation { get; set; }

    public virtual Zakaz? IdZakazNavigation { get; set; }
}
