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
public class RepairController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RepairMapper _mapper;

    public RepairController(AppDbContext context, AutoMapper.IMapper mapper)
    {
        _context = context;
        _mapper = new RepairMapper(mapper);
    }

    // GET: api/Repair
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Repair?>>> GetRepairs()
    {
        return await _context.Repairs.Select(repair => _mapper.Map(repair)).ToListAsync();
    }

    // GET: api/Repair/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Repair?>> GetRepair(Guid id)
    {
        var repair = await _context.Repairs.FindAsync(id);
        if (repair == null)
            return NotFound();

        return _mapper.Map(repair);
    }

    // PUT: api/Repair/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRepair(Guid id, Repair repair)
    {
        if (id != repair.Id)
            return BadRequest();

        _context.Entry(_mapper.Map(repair)!).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RepairExists(id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // POST: api/Repair
    [HttpPost]
    public async Task<ActionResult<Repair>> PostRepair(Repair repair)
    {
        repair.TotalJobs = repair.RepairJobs!.Count;

        foreach (var repairJob in repair.RepairJobs)
        {
            var job = await _context.Jobs.FindAsync(repairJob.JobId);
            if (job == null)
                continue;

            var timeSpan = job.Duration.ToTimeSpan();
            var timeOnly = new TimeOnly(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds).ToTimeSpan();
            
            repair.TotalTime = repair.TotalTime.Add(timeOnly);
            
            repair.TotalPrice += job.TotalPrice;
        }

        _context.Repairs.Add(_mapper.Map(repair)!);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRepair", new { id = repair.Id }, repair);
    }

    // DELETE: api/Repair/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRepair(Guid id)
    {
        var repair = await _context.Repairs.FindAsync(id);
        if (repair == null)
            return NotFound();

        _context.Repairs.Remove(repair);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RepairExists(Guid id)
    {
        return (_context.Repairs?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}