namespace PepeApi.Models;

public class BasketItem
{
    public int Id { get; set; }
    public int Count { get; set; }
    public int ProductId { get; set; }
    public string Title { get; set; } = "";
    public string Image { get; set; } = "";
    public decimal Cost { get; set; }
    public int Stock { get; set; }
}

public class OrderItemView
{
    public string Title { get; set; } = "";
    public int Count { get; set; }
}

public class OrderView
{
    public int Id { get; set; }
    public decimal PriceAll { get; set; }
    public string DateOrder { get; set; } = "";
    public string Status { get; set; } = "";
    public string AddressDelivery { get; set; } = "";
    public string DateDelivery { get; set; } = "";
    public List<OrderItemView> Items { get; set; } = new();
}
