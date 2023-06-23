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
public class JobController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JobMapper _mapper;

    public JobController(AppDbContext context, AutoMapper.IMapper mapper)
    {
        _context = context;
        _mapper = new JobMapper(mapper);
    }

    // GET: api/Job
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Job?>>> GetJobs()
    {
        return await _context.Jobs.Select(job => _mapper.Map(job)).ToListAsync();
    }

    // GET: api/Job/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Job?>> GetJob(Guid id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null)
            return NotFound();

        return _mapper.Map(job);
    }

    // PUT: api/Job/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJob(Guid id, Job job)
    {
        if (id != job.Id)
            return BadRequest();

        _context.Entry(_mapper.Map(job)!).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JobExists(id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    // POST: api/Job
    [HttpPost]
    public async Task<ActionResult<Job>> PostJob(Job job)
    {
        foreach (var jobItem in job.JobItems!)
        {
            var item = await _context.Items.FindAsync(jobItem.ItemId);
        
            if (item != null)
                job.TotalPrice += item.Price * jobItem.Quantity;
        }

        _context.Jobs.Add(_mapper.Map(job)!);

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetJob", new { id = job.Id }, job);
    }

    // DELETE: api/Job/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null)
            return NotFound();

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool JobExists(Guid id)
    {
        return (_context.Jobs?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}