using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly FlooresContext _db;

    public RolesController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll(string? search)
    {
        var query = _db.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.NameRole != null && x.NameRole.Contains(search));

        var list = await query
            .OrderBy(x => x.IdRole)
            .Select(x => new RoleDto
            {
                IdRole = x.IdRole,
                NameRole = x.NameRole
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(int id)
    {
        var item = await _db.Roles.AsNoTracking()
            .Where(x => x.IdRole == id)
            .Select(x => new RoleDto
            {
                IdRole = x.IdRole,
                NameRole = x.NameRole
            })
            .FirstOrDefaultAsync();

        if (item == null)
            return NotFound(new { message = "Роль не найдена" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(RoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NameRole))
            return BadRequest(new { message = "Укажите название роли" });

        if (dto.IdRole != 0 && await _db.Roles.AnyAsync(x => x.IdRole == dto.IdRole))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdRole == 0
            ? (await _db.Roles.MaxAsync(x => (int?)x.IdRole) ?? 0) + 1
            : dto.IdRole;

        _db.Roles.Add(new Role
        {
            IdRole = id,
            NameRole = dto.NameRole
        });
        await _db.SaveChangesAsync();

        dto.IdRole = id;
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> Update(int id, RoleDto dto)
    {
        var item = await _db.Roles.FirstOrDefaultAsync(x => x.IdRole == id);
        if (item == null)
            return NotFound(new { message = "Роль не найдена" });

        if (string.IsNullOrWhiteSpace(dto.NameRole))
            return BadRequest(new { message = "Укажите название роли" });

        item.NameRole = dto.NameRole;
        await _db.SaveChangesAsync();

        dto.IdRole = id;
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Roles.FirstOrDefaultAsync(x => x.IdRole == id);
        if (item == null)
            return NotFound(new { message = "Роль не найдена" });

        _db.Roles.Remove(item);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Нельзя удалить роль: она указана у пользователей" });
        }

        return Ok(new { message = "Удалено" });
    }
}
