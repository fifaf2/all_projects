using System;
using System.Collections.Generic;

namespace api_floower.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DataZakaz { get; set; }

    public int? CodePoly { get; set; }

    public int? IdStatus { get; set; }

    public virtual Status? IdStatusNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<Productorder> Productorders { get; set; } = new List<Productorder>();
}
