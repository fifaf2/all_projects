using api_floower.Dtos;
using api_floower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_floower.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PostavshiksController : ControllerBase
{
    private readonly FlooresContext _db;

    public PostavshiksController(FlooresContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<PostavshikDto>>> GetAll(string? search)
    {
        var query = _db.Postavshiks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x => x.NamePost != null && x.NamePost.Contains(search));

        var list = await query
            .OrderBy(x => x.IdPost)
            .Select(x => new PostavshikDto
            {
                IdPost = x.IdPost,
                NamePost = x.NamePost
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostavshikDto>> GetById(int id)
    {
        var item = await _db.Postavshiks.AsNoTracking()
            .Where(x => x.IdPost == id)
            .Select(x => new PostavshikDto
            {
                IdPost = x.IdPost,
                NamePost = x.NamePost
            })
            .FirstOrDefaultAsync();

        if (item == null)
            return NotFound(new { message = "Поставщик не найден" });

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<PostavshikDto>> Create(PostavshikDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NamePost))
            return BadRequest(new { message = "Укажите название поставщика" });

        if (dto.IdPost != 0 && await _db.Postavshiks.AnyAsync(x => x.IdPost == dto.IdPost))
            return Conflict(new { message = "Такой id уже есть" });

        var id = dto.IdPost == 0
            ? (await _db.Postavshiks.MaxAsync(x => (int?)x.IdPost) ?? 0) + 1
            : dto.IdPost;

        _db.Postavshiks.Add(new Postavshik
        {
            IdPost = id,
            NamePost = dto.NamePost
        });
        await _db.SaveChangesAsync();

        dto.IdPost = id;
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PostavshikDto>> Update(int id, PostavshikDto dto)
    {
        var item = await _db.Postavshiks.FirstOrDefaultAsync(x => x.IdPost == id);
        if (item == null)
            return NotFound(new { message = "Поставщик не найден" });

        if (string.IsNullOrWhiteSpace(dto.NamePost))
            return BadRequest(new { message = "Укажите название поставщика" });

        item.NamePost = dto.NamePost;
        await _db.SaveChangesAsync();

        dto.IdPost = id;
        return Ok(dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Postavshiks.FirstOrDefaultAsync(x => x.IdPost == id);
        if (item == null)
            return NotFound(new { message = "Поставщик не найден" });

        _db.Postavshiks.Remove(item);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { message = "Нельзя удалить поставщика: у него есть товары" });
        }

        return Ok(new { message = "Удалено" });
    }
}
