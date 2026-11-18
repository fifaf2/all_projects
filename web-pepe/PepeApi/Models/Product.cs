using System;
using System.Collections.Generic;

namespace PepeApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public int Count { get; set; }

    public int CategoryId { get; set; }

    public string? Status { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
