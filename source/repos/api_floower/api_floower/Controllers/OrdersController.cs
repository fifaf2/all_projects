using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly FlooresContext _db;

    public OrdersController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll(string? search, int? idStatus)
    {
        var query = _db.Orders.AsNoTracking();

        if (idStatus != null)
            query = query.Where(x => x.IdStatus == idStatus);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var byCode = int.TryParse(search, out var code);
            query = query.Where(x =>
                (x.IdUserNavigation != null && x.IdUserNavigation.Fio != null && x.IdUserNavigation.Fio.Contains(search)) ||
                (byCode && x.CodePoly == code));
        }

        var list = await query
            .OrderByDescending(x => x.DataZakaz)
            .Select(x => new OrderDto
            {
                IdOrder = x.IdOrder,
                IdUser = x.IdUser,
                Fio = x.IdUserNavigation != null ? x.IdUserNavigation.Fio : null,
                DataZakaz = x.DataZakaz,
                CodePoly = x.CodePoly,
                IdStatus = x.IdStatus,
                NameStatus = x.IdStatusNavigation != null ? x.IdStatusNavigation.NameStatus : null
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var item = await _db.Orders.AsNoTracking()
            .Where(x => x.IdOrder == id)
            .Select(x => new OrderDto
            {
                IdOrder = x.IdOrder,
                IdUser = x.IdUser,
                Fio = x.IdUserNavigation != null ? x.IdUserNavigation.Fio : null,
                DataZakaz = x.DataZakaz,
                CodePoly = x.CodePoly,
                IdStatus = x.IdStatus,
                NameStatus = x.IdStatusNavigation != null ? x.IdStatusNavigation.NameStatus : null
            })
            .FirstOrDefaultAsync();

        if (item == null)
            return NotFound(new { message = "Заказ не найден" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(OrderDto dto)
    {
        if (dto.IdUser != null && !await _db.Users.AnyAsync(x => x.IdUser == dto.IdUser))
            return BadRequest(new { message = "Клиент не найден" });

        if (dto.IdStatus != null && !await _db.Statuses.AnyAsync(x => x.IdStatus == dto.IdStatus))
            return BadRequest(new { message = "Статус не найден" });

        if (dto.IdOrder != 0 && await _db.Orders.AnyAsync(x => x.IdOrder == dto.IdOrder))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdOrder == 0
            ? (await _db.Orders.MaxAsync(x => (int?)x.IdOrder) ?? 0) + 1
            : dto.IdOrder;

        _db.Orders.Add(new Order
        {
            IdOrder = id,
            IdUser = dto.IdUser,
            DataZakaz = dto.DataZakaz ?? DateTime.Now,
            CodePoly = dto.CodePoly,
            IdStatus = dto.IdStatus
        });
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id }, await Load(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderDto>> Update(int id, OrderDto dto)
    {
        var item = await _db.Orders.FirstOrDefaultAsync(x => x.IdOrder == id);
        if (item == null)
            return NotFound(new { message = "Заказ не найден" });

        if (dto.IdUser != null && !await _db.Users.AnyAsync(x => x.IdUser == dto.IdUser))
            return BadRequest(new { message = "Клиент не найден" });

        if (dto.IdStatus != null && !await _db.Statuses.AnyAsync(x => x.IdStatus == dto.IdStatus))
            return BadRequest(new { message = "Статус не найден" });

        item.IdUser = dto.IdUser;
        item.DataZakaz = dto.DataZakaz ?? item.DataZakaz;
        item.CodePoly = dto.CodePoly;
        item.IdStatus = dto.IdStatus;
        await _db.SaveChangesAsync();

        return Ok(await Load(id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Orders.FirstOrDefaultAsync(x => x.IdOrder == id);
        if (item == null)
            return NotFound(new { message = "Заказ не найден" });

        var lines = await _db.Productorders.Where(x => x.IdOrder == id).ToListAsync();
        _db.Productorders.RemoveRange(lines);
        _db.Orders.Remove(item);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Удалено" });
    }

    private Task<OrderDto?> Load(int id)
    {
        return _db.Orders.AsNoTracking()
            .Where(x => x.IdOrder == id)
            .Select(x => new OrderDto
            {
                IdOrder = x.IdOrder,
                IdUser = x.IdUser,
                Fio = x.IdUserNavigation != null ? x.IdUserNavigation.Fio : null,
                DataZakaz = x.DataZakaz,
                CodePoly = x.CodePoly,
                IdStatus = x.IdStatus,
                NameStatus = x.IdStatusNavigation != null ? x.IdStatusNavigation.NameStatus : null
            })
            .FirstOrDefaultAsync();
    }
}
