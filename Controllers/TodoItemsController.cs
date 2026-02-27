using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TappApi.Models;

namespace TappApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        var items = await _context.TodoItems.AsNoTracking().ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem dto)
    {
        _context.TodoItems.Add(dto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TodoItem dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID in route does not match body.");
        }

        if (!await _context.TodoItems.AnyAsync(t => t.Id == id))
        {
            return NotFound();
        }

        _context.Entry(dto).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _context.TodoItems.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(existing);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
