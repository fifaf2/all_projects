using System;
using System.Collections.Generic;

namespace api_floower.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string? NameProduct { get; set; }

    public int? IdPost { get; set; }

    public int? Sell { get; set; }

    public int? Kolvo { get; set; }

    public string? Description { get; set; }

    public virtual Postavshik? IdPostNavigation { get; set; }

    public virtual ICollection<Productorder> Productorders { get; set; } = new List<Productorder>();
}
