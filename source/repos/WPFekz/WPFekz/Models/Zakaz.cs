using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Zakaz
{
    public int IdZakaz { get; set; }

    public DateOnly? DateZakaz { get; set; }

    public DateOnly? DataDost { get; set; }

    public int? IdPunkt { get; set; }

    public int? IdUser { get; set; }

    public int? Code { get; set; }

    public int? IdStatus { get; set; }

    public virtual Punkti? IdPunktNavigation { get; set; }

    public virtual Status? IdStatusNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<Tovzakaz> Tovzakazs { get; set; } = new List<Tovzakaz>();
}
