using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StatusesController : ControllerBase
{
    private readonly FlooresContext _db;

    public StatusesController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<StatusDto>>> GetAll(string? search)
    {
        var query = _db.Statuses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.NameStatus != null && x.NameStatus.Contains(search));

        var list = await query
            .OrderBy(x => x.IdStatus)
            .Select(x => new StatusDto
            {
                IdStatus = x.IdStatus,
                NameStatus = x.NameStatus
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StatusDto>> GetById(int id)
    {
        var item = await _db.Statuses.AsNoTracking()
            .Where(x => x.IdStatus == id)
            .Select(x => new StatusDto
            {
                IdStatus = x.IdStatus,
                NameStatus = x.NameStatus
            })
            .FirstOrDefaultAsync();

        if (item == null)
            return NotFound(new { message = "Статус не найден" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<StatusDto>> Create(StatusDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NameStatus))
            return BadRequest(new { message = "Укажите название статуса" });

        if (dto.IdStatus != 0 && await _db.Statuses.AnyAsync(x => x.IdStatus == dto.IdStatus))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdStatus == 0
            ? (await _db.Statuses.MaxAsync(x => (int?)x.IdStatus) ?? 0) + 1
            : dto.IdStatus;

        _db.Statuses.Add(new Status
        {
            IdStatus = id,
            NameStatus = dto.NameStatus
        });
        await _db.SaveChangesAsync();

        dto.IdStatus = id;
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StatusDto>> Update(int id, StatusDto dto)
    {
        var item = await _db.Statuses.FirstOrDefaultAsync(x => x.IdStatus == id);
        if (item == null)
            return NotFound(new { message = "Статус не найден" });

        if (string.IsNullOrWhiteSpace(dto.NameStatus))
            return BadRequest(new { message = "Укажите название статуса" });

        item.NameStatus = dto.NameStatus;
        await _db.SaveChangesAsync();

        dto.IdStatus = id;
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Statuses.FirstOrDefaultAsync(x => x.IdStatus == id);
        if (item == null)
            return NotFound(new { message = "Статус не найден" });

        _db.Statuses.Remove(item);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Нельзя удалить статус" });
        }

        return Ok(new { message = "Удалено" });
    }
}
