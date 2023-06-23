using System.Net.Mime;
using App.DAL;
using App.Public.DTO;
using App.Public.DTO.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ItemController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ItemMapper _mapper;

    public ItemController(AppDbContext context, AutoMapper.IMapper mapper)
    {
        _context = context;
        _mapper = new ItemMapper(mapper);
    }

    // GET: api/Item
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item?>>> GetItems()
    {
        return await _context.Items.Select(item => _mapper.Map(item)).ToListAsync();
    }

    // GET: api/Item/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Item?>> GetItem(Guid id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
            return NotFound();

        return _mapper.Map(item);
    }

    // PUT: api/Item/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(Guid id, Item item)
    {
        if (id != item.Id)
            return BadRequest();

        _context.Entry(_mapper.Map(item)!).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ItemExists(id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // POST: api/Item
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(Item item)
    {
        _context.Items.Add(_mapper.Map(item)!);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetItem", new { id = item.Id }, item);
    }

    // DELETE: api/Item/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
            return NotFound();

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ItemExists(Guid id)
    {
        return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}