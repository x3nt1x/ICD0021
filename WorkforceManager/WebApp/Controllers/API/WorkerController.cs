using System.Net.Mime;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using App.Public.DTO.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers.API;

/// <summary>
/// API endpoint for managing workers
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WorkerController : ControllerBase
{
    private readonly IAppBll _appBll;
    private readonly WorkerMapper _mapper;

    public WorkerController(IAppBll appBll, AutoMapper.IMapper mapper)
    {
        _appBll = appBll;
        _mapper = new WorkerMapper(mapper);
    }

    // GET: api/<version>/Worker
    /// <summary>Get all workers</summary>
    /// <returns>List of workers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Worker>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
    {
        return (await _appBll.Workers.AllAsync()).Select(worker => _mapper.Map(worker)).ToList()!;
    }

    // GET: api/<version>/Worker/<id>
    /// <summary>Get single worker</summary>
    /// <param name="id">ID of worker</param>
    /// <returns>Requested worker</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Worker), StatusCodes.Status200OK)]
    public async Task<ActionResult<Worker>> GetWorker(Guid? id)
    {
        if (id == null)
            return NotFound();

        var worker = await _appBll.Workers.FirstOrDefaultAsync(id.Value);
        if (worker == null)
            return NotFound();

        return _mapper.Map(worker)!;
    }

    // PUT: api/<version>/Worker/<id>
    /// <summary>Update worker</summary>
    /// <param name="id">ID of worker</param>
    /// <param name="worker">New data to insert</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutWorker(Guid id, Worker worker)
    {
        if (id != worker.Id)
            return BadRequest();

        _appBll.Workers.Update(_mapper.Map(worker)!);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/<version>/Worker
    /// <summary>Create worker</summary>
    /// <param name="worker">New worker data</param>
    /// <returns>Created worker</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Worker), StatusCodes.Status201Created)]
    public async Task<ActionResult<Worker>> PostWorker(Worker worker)
    {
        worker.Id = new Guid();

        _appBll.Workers.Add(_mapper.Map(worker)!);

        await _appBll.SaveChangesAsync();

        return CreatedAtAction("GetWorker", new { id = worker.Id }, worker);
    }

    // POST: api/<version>/Worker/AddMultiple
    /// <summary>Create multiple workers</summary>
    /// <param name="workers">List of workers to create</param>
    /// <returns>No content</returns>
    [HttpPost("AddMultiple")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> PostWorkers(List<Worker> workers)
    {
        foreach (var worker in workers)
        {
            worker.Id = new Guid();

            _appBll.Workers.Add(_mapper.Map(worker)!);
        }

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/<version>/Worker/<id>
    /// <summary>Delete worker</summary>
    /// <param name="id">ID of worker</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteWorker(Guid id)
    {
        _appBll.Workers.Remove(id);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }
}