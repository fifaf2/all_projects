using System;
using System.Collections.Generic;

namespace PepeApi.Models;

public partial class Basket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public virtual User User { get; set; } = null!;
}
