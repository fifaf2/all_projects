using System;
using System.Collections.Generic;

namespace api_floower.Models;

public partial class Postavshik
{
    public int IdPost { get; set; }

    public string? NamePost { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
