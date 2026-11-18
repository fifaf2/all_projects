using Microsoft.AspNetCore.Mvc;
using PepeApi.Data;
using PepeApi.Models;

namespace PepeApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BasketController : ControllerBase
{
    private readonly PepeDb db;

    public BasketController(PepeDb db)
    {
        this.db = db;
    }

    [HttpGet]
    public IActionResult GetBasket(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        try
        {
            var list = (
                from basket in db.Baskets
                join product in db.Products on basket.ProductId equals product.Id
                where basket.UserId == userId
                select new BasketItem
                {
                    Id = basket.Id,
                    Count = basket.Count,
                    ProductId = product.Id,
                    Title = product.Title,
                    Image = product.Image,
                    Cost = product.Cost,
                    Stock = product.Count
                }
            ).ToList();

            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPost]
    public IActionResult PostAddToBasket(int userId, int productId, int count)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        if (count < 1)
        {
            count = 1;
        }

        try
        {
            var product = (
                from p in db.Products
                where p.Id == productId && p.Status == "Активен"
                select p
            ).FirstOrDefault();

            if (product == null)
            {
                return BadRequest(new { message = "Товар не найден" });
            }

            var existing = (
                from b in db.Baskets
                where b.UserId == userId && b.ProductId == productId
                select b
            ).FirstOrDefault();

            int already = existing == null ? 0 : existing.Count;
            if (already + count > product.Count)
            {
                return BadRequest(new { message = "Недостаточно товара на складе" });
            }

            if (existing != null)
            {
                existing.Count = already + count;
            }
            else
            {
                db.Baskets.Add(new Basket
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = count
                });
            }

            db.SaveChanges();
            return Ok(new { message = "Товар добавлен в корзину" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPut]
    public IActionResult PutUpdateBasket(int userId, int basketId, int count)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        if (count < 1)
        {
            count = 1;
        }

        try
        {
            var row = (
                from basket in db.Baskets
                join product in db.Products on basket.ProductId equals product.Id
                where basket.Id == basketId && basket.UserId == userId
                select new { basket, product }
            ).FirstOrDefault();

            if (row == null)
            {
                return BadRequest(new { message = "Позиция корзины не найдена" });
            }

            if (count > row.product.Count)
            {
                return BadRequest(new { message = "Недостаточно товара на складе" });
            }

            row.basket.Count = count;
            db.SaveChanges();
            return Ok(new { message = "Количество обновлено" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPost]
    public IActionResult PostRemoveFromBasket(int userId, int basketId)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        try
        {
            var basket = (
                from b in db.Baskets
                where b.Id == basketId && b.UserId == userId
                select b
            ).FirstOrDefault();

            if (basket != null)
            {
                db.Baskets.Remove(basket);
                db.SaveChanges();
            }

            return Ok(new { message = "Товар удалён из корзины" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }
}
