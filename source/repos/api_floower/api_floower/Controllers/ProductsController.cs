using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly FlooresContext _db;

    public ProductsController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll(string? search)
    {
        var query = _db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                (x.NameProduct != null && x.NameProduct.Contains(search)) ||
                (x.Description != null && x.Description.Contains(search)));
        }

        var list = await query
            .OrderBy(x => x.IdProduct)
            .Select(x => new ProductDto
            {
                IdProduct = x.IdProduct,
                NameProduct = x.NameProduct,
                IdPost = x.IdPost,
                NamePost = x.IdPostNavigation != null ? x.IdPostNavigation.NamePost : null,
                Sell = x.Sell,
                Kolvo = x.Kolvo,
                Description = x.Description
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var item = await _db.Products.AsNoTracking()
            .Where(x => x.IdProduct == id)
            .Select(x => new ProductDto
            {
                IdProduct = x.IdProduct,
                NameProduct = x.NameProduct,
                IdPost = x.IdPost,
                NamePost = x.IdPostNavigation != null ? x.IdPostNavigation.NamePost : null,
                Sell = x.Sell,
                Kolvo = x.Kolvo,
                Description = x.Description
            })
            .FirstOrDefaultAsync();

        if (item == null)
            return NotFound(new { message = "Товар не найден" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(ProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NameProduct))
            return BadRequest(new { message = "Укажите название товара" });

        if (dto.IdPost != null && !await _db.Postavshiks.AnyAsync(x => x.IdPost == dto.IdPost))
            return BadRequest(new { message = "Поставщик не найден" });

        if (dto.IdProduct != 0 && await _db.Products.AnyAsync(x => x.IdProduct == dto.IdProduct))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdProduct == 0
            ? (await _db.Products.MaxAsync(x => (int?)x.IdProduct) ?? 0) + 1
            : dto.IdProduct;

        _db.Products.Add(new Product
        {
            IdProduct = id,
            NameProduct = dto.NameProduct,
            IdPost = dto.IdPost,
            Sell = dto.Sell,
            Kolvo = dto.Kolvo,
            Description = dto.Description
        });
        await _db.SaveChangesAsync();

        dto.IdProduct = id;
        dto.NamePost = dto.IdPost == null
            ? null
            : await _db.Postavshiks.Where(x => x.IdPost == dto.IdPost).Select(x => x.NamePost).FirstOrDefaultAsync();

        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(int id, ProductDto dto)
    {
        var item = await _db.Products.FirstOrDefaultAsync(x => x.IdProduct == id);
        if (item == null)
            return NotFound(new { message = "Товар не найден" });

        if (string.IsNullOrWhiteSpace(dto.NameProduct))
            return BadRequest(new { message = "Укажите название товара" });

        if (dto.IdPost != null && !await _db.Postavshiks.AnyAsync(x => x.IdPost == dto.IdPost))
            return BadRequest(new { message = "Поставщик не найден" });

        item.NameProduct = dto.NameProduct;
        item.IdPost = dto.IdPost;
        item.Sell = dto.Sell;
        item.Kolvo = dto.Kolvo;
        item.Description = dto.Description;
        await _db.SaveChangesAsync();

        dto.IdProduct = id;
        dto.NamePost = dto.IdPost == null
            ? null
            : await _db.Postavshiks.Where(x => x.IdPost == dto.IdPost).Select(x => x.NamePost).FirstOrDefaultAsync();

        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Products.FirstOrDefaultAsync(x => x.IdProduct == id);
        if (item == null)
            return NotFound(new { message = "Товар не найден" });

        _db.Products.Remove(item);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Нельзя удалить товар: он есть в заказах" });
        }

        return Ok(new { message = "Удалено" });
    }
}
