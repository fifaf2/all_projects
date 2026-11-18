using System;
using System.Collections.Generic;

namespace PepeApi.Models;

public partial class Order
{
    public int Id { get; set; }

    public decimal PriceAll { get; set; }

    public DateTime? DateOrder { get; set; }

    public int UserId { get; set; }

    public string? Status { get; set; }

    public string AddressDelivery { get; set; } = null!;

    public DateOnly DateDelivery { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual User User { get; set; } = null!;
}
