using System;
using System.Collections.Generic;

namespace api_floower.Models;

public partial class Productorder
{
    public int IdProductorders { get; set; }

    public int? IdProduct { get; set; }

    public int? Kolvo { get; set; }

    public int? IdOrder { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
