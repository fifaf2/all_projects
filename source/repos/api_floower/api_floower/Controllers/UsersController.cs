using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly FlooresContext _db;

    public UsersController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll(string? search)
    {
        var query = _db.Users.AsNoTracking().Include(x => x.IdRoleNavigation).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                (x.Fio != null && x.Fio.Contains(search)) ||
                (x.Login != null && x.Login.Contains(search)));
        }

        var list = await query
            .OrderBy(x => x.IdUser)
            .Select(x => new UserDto
            {
                IdUser = x.IdUser,
                Fio = x.Fio,
                Login = x.Login,
                Password = x.Password,
                IdRole = x.IdRole,
                NameRole = x.IdRoleNavigation != null ? x.IdRoleNavigation.NameRole : null
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var item = await _db.Users.AsNoTracking()
            .Where(x => x.IdUser == id)
            .Select(x => new UserDto
            {
                IdUser = x.IdUser,
                Fio = x.Fio,
                Login = x.Login,
                Password = x.Password,
                IdRole = x.IdRole,
                NameRole = x.IdRoleNavigation != null ? x.IdRoleNavigation.NameRole : null
            })
            .FirstOrDefaultAsync();

        if (item == null)
            return NotFound(new { message = "Пользователь не найден" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(UserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Fio))
            return BadRequest(new { message = "Укажите ФИО" });

        if (dto.IdRole != null && !await _db.Roles.AnyAsync(x => x.IdRole == dto.IdRole))
            return BadRequest(new { message = "Роль не найдена" });

        if (dto.IdUser != 0 && await _db.Users.AnyAsync(x => x.IdUser == dto.IdUser))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdUser == 0
            ? (await _db.Users.MaxAsync(x => (int?)x.IdUser) ?? 0) + 1
            : dto.IdUser;

        _db.Users.Add(new User
        {
            IdUser = id,
            Fio = dto.Fio,
            Login = dto.Login,
            Password = dto.Password,
            IdRole = dto.IdRole
        });
        await _db.SaveChangesAsync();

        dto.IdUser = id;
        dto.NameRole = dto.IdRole == null
            ? null
            : await _db.Roles.Where(x => x.IdRole == dto.IdRole).Select(x => x.NameRole).FirstOrDefaultAsync();

        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(int id, UserDto dto)
    {
        var item = await _db.Users.FirstOrDefaultAsync(x => x.IdUser == id);
        if (item == null)
            return NotFound(new { message = "Пользователь не найден" });

        if (string.IsNullOrWhiteSpace(dto.Fio))
            return BadRequest(new { message = "Укажите ФИО" });

        if (dto.IdRole != null && !await _db.Roles.AnyAsync(x => x.IdRole == dto.IdRole))
            return BadRequest(new { message = "Роль не найдена" });

        item.Fio = dto.Fio;
        item.Login = dto.Login;
        item.Password = dto.Password;
        item.IdRole = dto.IdRole;
        await _db.SaveChangesAsync();

        dto.IdUser = id;
        dto.NameRole = dto.IdRole == null
            ? null
            : await _db.Roles.Where(x => x.IdRole == dto.IdRole).Select(x => x.NameRole).FirstOrDefaultAsync();

        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Users.FirstOrDefaultAsync(x => x.IdUser == id);
        if (item == null)
            return NotFound(new { message = "Пользователь не найден" });

        _db.Users.Remove(item);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Нельзя удалить пользователя: у него есть заказы" });
        }

        return Ok(new { message = "Удалено" });
    }
}
