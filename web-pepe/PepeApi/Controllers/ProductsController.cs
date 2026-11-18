using Microsoft.AspNetCore.Mvc;
using PepeApi.Data;
using PepeApi.Models;

namespace PepeApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly PepeDb db;

    public ProductsController(PepeDb db)
    {
        this.db = db;
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
        try
        {
            var list = (
                from category in db.Categories
                orderby category.Name
                select category
            ).ToList();

            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetProducts(int categoryId = 0)
    {
        try
        {
            var list = (
                from product in db.Products
                where product.Status == "Активен"
                    && (categoryId == 0 || product.CategoryId == categoryId)
                orderby product.Id
                select product
            ).ToList();

            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetProduct(int id)
    {
        try
        {
            var product = (
                from p in db.Products
                where p.Id == id && p.Status == "Активен"
                select p
            ).FirstOrDefault();

            if (product == null)
            {
                return BadRequest(new { message = "Товар не найден" });
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPost]
    public IActionResult PostAddProduct(
        string? title,
        string? image,
        string? description,
        decimal cost,
        int count,
        int categoryId)
    {
        title = (title ?? "").Trim();
        image = (image ?? "").Trim();
        description = (description ?? "").Trim();

        if (title == "" || image == "" || description == "")
        {
            return BadRequest(new { message = "Заполните название, картинку и описание" });
        }

        if (title.Length > 100)
        {
            return BadRequest(new { message = "Название не длиннее 100 символов" });
        }

        if (cost < 0 || count < 0)
        {
            return BadRequest(new { message = "Цена и количество не могут быть отрицательными" });
        }

        try
        {
            bool categoryExists = (
                from category in db.Categories
                where category.Id == categoryId
                select category
            ).Any();

            if (!categoryExists)
            {
                return BadRequest(new { message = "Категория не найдена" });
            }

            var product = new Product
            {
                Title = title,
                Image = image,
                Description = description,
                Cost = cost,
                Count = count,
                CategoryId = categoryId,
                Status = "Активен"
            };
            db.Products.Add(product);
            db.SaveChanges();
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPut]
    public IActionResult PutUpdateProduct(
        int id,
        string? title,
        string? image,
        string? description,
        decimal cost,
        int count,
        int categoryId)
    {
        title = (title ?? "").Trim();
        image = (image ?? "").Trim();
        description = (description ?? "").Trim();

        if (title == "" || image == "" || description == "")
        {
            return BadRequest(new { message = "Заполните название, картинку и описание" });
        }

        if (title.Length > 100)
        {
            return BadRequest(new { message = "Название не длиннее 100 символов" });
        }

        if (cost < 0 || count < 0)
        {
            return BadRequest(new { message = "Цена и количество не могут быть отрицательными" });
        }

        try
        {
            var product = (
                from p in db.Products
                where p.Id == id
                select p
            ).FirstOrDefault();

            if (product == null)
            {
                return BadRequest(new { message = "Товар не найден" });
            }

            bool categoryExists = (
                from category in db.Categories
                where category.Id == categoryId
                select category
            ).Any();

            if (!categoryExists)
            {
                return BadRequest(new { message = "Категория не найдена" });
            }

            product.Title = title;
            product.Image = image;
            product.Description = description;
            product.Cost = cost;
            product.Count = count;
            product.CategoryId = categoryId;
            db.SaveChanges();
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpDelete]
    public IActionResult DeleteProduct(int id)
    {
        try
        {
            var product = (
                from p in db.Products
                where p.Id == id
                select p
            ).FirstOrDefault();

            if (product == null || product.Status == "Удалён")
            {
                return BadRequest(new { message = "Товар не найден" });
            }

            product.Status = "Удалён";
            db.SaveChanges();
            return Ok(new { message = "Товар удалён" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }
}
