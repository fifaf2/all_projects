using Microsoft.AspNetCore.Mvc;
using PepeApi.Data;
using PepeApi.Models;

namespace PepeApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class OrdersController : ControllerBase
{
    private readonly PepeDb db;

    public OrdersController(PepeDb db)
    {
        this.db = db;
    }

    [HttpGet]
    public IActionResult GetOrders(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        try
        {
            var orders = (
                from order in db.Orders
                where order.UserId == userId
                orderby order.Id descending
                select order
            ).ToList();

            var result = new List<OrderView>();
            foreach (var order in orders)
            {
                var items = (
                    from item in db.Orderitems
                    join product in db.Products on item.ProductId equals product.Id
                    where item.OrderId == order.Id
                    select new OrderItemView
                    {
                        Title = product.Title,
                        Count = item.Count
                    }
                ).ToList();

                result.Add(new OrderView
                {
                    Id = order.Id,
                    PriceAll = order.PriceAll,
                    DateOrder = order.DateOrder == null ? "" : order.DateOrder.Value.ToString("dd.MM.yyyy HH:mm"),
                    Status = order.Status ?? "",
                    AddressDelivery = order.AddressDelivery,
                    DateDelivery = order.DateDelivery.ToString("dd.MM.yyyy"),
                    Items = items
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPost]
    public IActionResult PostCreateOrder(int userId, string? address, string? dateDelivery)
    {
        address = (address ?? "").Trim();
        dateDelivery = (dateDelivery ?? "").Trim();

        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        if (address == "" || dateDelivery == "")
        {
            return BadRequest(new { message = "Укажите адрес и дату доставки" });
        }

        if (!DateOnly.TryParse(dateDelivery, out var deliveryDate))
        {
            return BadRequest(new { message = "Укажите дату доставки" });
        }

        try
        {
            var rows = (
                from basket in db.Baskets
                join product in db.Products on basket.ProductId equals product.Id
                where basket.UserId == userId
                select new { basket, product }
            ).ToList();

            if (rows.Count == 0)
            {
                return BadRequest(new { message = "Корзина пуста" });
            }

            decimal total = 0;
            foreach (var row in rows)
            {
                if (row.basket.Count > row.product.Count)
                {
                    return BadRequest(new { message = "Недостаточно товара: " + row.product.Title });
                }

                total += row.product.Cost * row.basket.Count;
            }

            var order = new Order
            {
                PriceAll = total,
                DateOrder = DateTime.Now,
                UserId = userId,
                Status = "Новый",
                AddressDelivery = address,
                DateDelivery = deliveryDate
            };
            db.Orders.Add(order);
            db.SaveChanges();

            foreach (var row in rows)
            {
                db.Orderitems.Add(new Orderitem
                {
                    OrderId = order.Id,
                    ProductId = row.product.Id,
                    Count = row.basket.Count
                });
                row.product.Count = row.product.Count - row.basket.Count;
                db.Baskets.Remove(row.basket);
            }

            db.SaveChanges();
            return Ok(new { message = "Заказ №" + order.Id + " успешно оформлен!", orderId = order.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }
}
