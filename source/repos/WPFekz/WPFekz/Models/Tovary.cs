using System;
using System.Collections.Generic;

namespace WPFekz.Models;

public partial class Tovary
{
    public int Id { get; set; }

    public string? Articule { get; set; }

    public string? NameTovary { get; set; }

    public int? IdEd { get; set; }

    public int? Sell { get; set; }

    public int? IdPost { get; set; }

    public int? IdManufac { get; set; }

    public int? IdCategory { get; set; }

    public int? Scidka { get; set; }

    public int? Kolvo { get; set; }

    public string? OpisTovar { get; set; }

    public string? Photo { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Edinici? IdEdNavigation { get; set; }

    public virtual Manufacture? IdManufacNavigation { get; set; }

    public virtual Postavshiki? IdPostNavigation { get; set; }

    public virtual ICollection<Tovzakaz> Tovzakazs { get; set; } = new List<Tovzakaz>();
}
