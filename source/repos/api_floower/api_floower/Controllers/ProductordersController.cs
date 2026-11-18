using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductordersController : ControllerBase
{
    private readonly FlooresContext _db;

    public ProductordersController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductorderDto>>> GetAll(int? idOrder, int? idProduct)
    {
        var query = _db.Productorders.AsNoTracking();

        if (idOrder != null)
            query = query.Where(x => x.IdOrder == idOrder);

        if (idProduct != null)
            query = query.Where(x => x.IdProduct == idProduct);

        var list = await query
            .OrderBy(x => x.IdProductorders)
            .Select(x => new ProductorderDto
            {
                IdProductorders = x.IdProductorders,
                IdProduct = x.IdProduct,
                NameProduct = x.IdProductNavigation != null ? x.IdProductNavigation.NameProduct : null,
                Kolvo = x.Kolvo,
                IdOrder = x.IdOrder
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductorderDto>> GetById(int id)
    {
        var item = await Load(id);
        if (item == null)
            return NotFound(new { message = "Позиция не найдена" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ProductorderDto>> Create(ProductorderDto dto)
    {
        if (dto.IdOrder == null || !await _db.Orders.AnyAsync(x => x.IdOrder == dto.IdOrder))
            return BadRequest(new { message = "Заказ не найден" });

        if (dto.IdProduct == null || !await _db.Products.AnyAsync(x => x.IdProduct == dto.IdProduct))
            return BadRequest(new { message = "Товар не найден" });

        if (dto.Kolvo == null || dto.Kolvo <= 0)
            return BadRequest(new { message = "Укажите количество больше нуля" });

        if (dto.IdProductorders != 0 && await _db.Productorders.AnyAsync(x => x.IdProductorders == dto.IdProductorders))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdProductorders == 0
            ? (await _db.Productorders.MaxAsync(x => (int?)x.IdProductorders) ?? 0) + 1
            : dto.IdProductorders;

        _db.Productorders.Add(new Productorder
        {
            IdProductorders = id,
            IdProduct = dto.IdProduct,
            Kolvo = dto.Kolvo,
            IdOrder = dto.IdOrder
        });
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id }, await Load(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductorderDto>> Update(int id, ProductorderDto dto)
    {
        var item = await _db.Productorders.FirstOrDefaultAsync(x => x.IdProductorders == id);
        if (item == null)
            return NotFound(new { message = "Позиция не найдена" });

        if (dto.IdOrder == null || !await _db.Orders.AnyAsync(x => x.IdOrder == dto.IdOrder))
            return BadRequest(new { message = "Заказ не найден" });

        if (dto.IdProduct == null || !await _db.Products.AnyAsync(x => x.IdProduct == dto.IdProduct))
            return BadRequest(new { message = "Товар не найден" });

        if (dto.Kolvo == null || dto.Kolvo <= 0)
            return BadRequest(new { message = "Укажите количество больше нуля" });

        item.IdProduct = dto.IdProduct;
        item.Kolvo = dto.Kolvo;
        item.IdOrder = dto.IdOrder;
        await _db.SaveChangesAsync();

        return Ok(await Load(id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Productorders.FirstOrDefaultAsync(x => x.IdProductorders == id);
        if (item == null)
            return NotFound(new { message = "Позиция не найдена" });

        _db.Productorders.Remove(item);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Удалено" });
    }

    private Task<ProductorderDto?> Load(int id)
    {
        return _db.Productorders.AsNoTracking()
            .Where(x => x.IdProductorders == id)
            .Select(x => new ProductorderDto
            {
                IdProductorders = x.IdProductorders,
                IdProduct = x.IdProduct,
                NameProduct = x.IdProductNavigation != null ? x.IdProductNavigation.NameProduct : null,
                Kolvo = x.Kolvo,
                IdOrder = x.IdOrder
            })
            .FirstOrDefaultAsync();
    }
}
