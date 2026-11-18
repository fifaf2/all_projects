using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FlooresContext _db;

    public AuthController(FlooresContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { message = "Укажите логин и пароль" });

        var user = await _db.Users
            .Include(x => x.IdRoleNavigation)
            .FirstOrDefaultAsync(x => x.Login == dto.Login && x.Password == dto.Password);

        if (user == null)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        return Ok(new UserDto
        {
            IdUser = user.IdUser,
            Fio = user.Fio,
            Login = user.Login,
            IdRole = user.IdRole,
            NameRole = user.IdRoleNavigation?.NameRole
        });
    }
}
