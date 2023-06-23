using System.Net.Mime;
using App.Contracts.BLL;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers.API;

/// <summary>
/// API endpoint for managing assignments
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AssignmentController : ControllerBase
{
    private readonly IAppBll _appBll;
    private readonly AssignmentMapper _mapper;
    private readonly CommentMapper _commentMapper;
    private readonly WorkerMapper _workerMapper;

    public AssignmentController(IAppBll appBll, AutoMapper.IMapper mapper)
    {
        _appBll = appBll;
        _mapper = new AssignmentMapper(mapper);
        _commentMapper = new CommentMapper(mapper);
        _workerMapper = new WorkerMapper(mapper);
    }

    // GET: api/<version>/Assignment
    /// <summary>Get all assignments of a user</summary>
    /// <returns>List of user assignments</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Assignment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments()
    {
        return (await _appBll.Assignments.GetUserAssignmentsAsync(User.GetUserId()))
            .Select(assignment => _mapper.Map(assignment)!)
            .ToList();
    }

    // GET: api/<version>/Assignment/<id>
    /// <summary>Get single assignment of a user</summary>
    /// <param name="id">ID of assignment</param>
    /// <returns>Requested assignment</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Assignment), StatusCodes.Status200OK)]
    public async Task<ActionResult<Assignment>> GetAssignment(Guid id)
    {
        if (!_appBll.Assignments.IsUserAssignment(id, User.GetUserId()))
            return Unauthorized();

        var assignment = await _appBll.Assignments.GetAssignmentAsync(id);
        if (assignment == null)
            return NotFound();

        return _mapper.Map(assignment)!;
    }

    // GET: api/<version>/Assignment/<id>/Comments
    /// <summary>Get all assignment comments</summary>
    /// <param name="id">ID of assignment</param>
    /// <returns>List of assignment comments</returns>
    [HttpGet("{id}/Comments")]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IEnumerable<Comment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Comment>>> GetAssignmentComments(Guid id)
    {
        if (!_appBll.Assignments.IsUserAssignment(id, User.GetUserId()))
            return Unauthorized();

        return (await _appBll.Comments.GetAssignmentCommentsAsync(id))
            .Select(comment => _commentMapper.MapWithAppUser(comment))
            .ToList();
    }

    // GET: api/<version>/Assignment/<id>/Workers
    /// <summary>Get all assignment workers</summary>
    /// <param name="id">ID of assignment</param>
    /// <returns>List of assignment workers</returns>
    [HttpGet("{id}/Workers")]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IEnumerable<Worker>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Worker>>> GetAssignmentWorkers(Guid id)
    {
        if (!_appBll.Assignments.IsUserAssignment(id, User.GetUserId()))
            return Unauthorized();

        return (await _appBll.Workers.GetAssignmentWorkersAsync(id))
            .Select(worker => _workerMapper.MapWithAppUser(worker))
            .ToList();
    }

    // PUT: api/<version>/Assignment/<id>
    /// <summary>Update assignment</summary>
    /// <param name="id">ID of assignment</param>
    /// <param name="assignment">New data to insert</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutAssignment(Guid id, Assignment assignment)
    {
        if (id != assignment.Id)
            return BadRequest();

        if (!_appBll.Assignments.IsUserAssignment(id, User.GetUserId()))
            return Unauthorized();

        _appBll.Assignments.Update(_mapper.Map(assignment)!);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/<version>/Assignment
    /// <summary>Create assignment</summary>
    /// <param name="assignment">New assignment data</param>
    /// <returns>Created assignment</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Assignment), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Assignment>> PostAssignment(Assignment assignment)
    {
        assignment.Id = new Guid();
        assignment.Comments = new List<Comment>();
        assignment.Workers = new List<Worker>
        {
            new Worker
            {
                Id = new Guid(),
                AssignmentId = assignment.Id,
                AppUserId = User.GetUserId()
            }
        };

        _appBll.Assignments.Add(_mapper.Map(assignment)!);

        var order = await _appBll.Orders.FirstOrDefaultAsync(assignment.OrderId);
        if (order == null)
            return NotFound();
        
        var client = await _appBll.Clients.FirstOrDefaultAsync(order.ClientId);
        if (client == null)
            return NotFound();

        order.TotalTasks++;
        client.TotalTasks++;
        
        _appBll.Orders.Update(order);
        _appBll.Clients.Update(client);
        
        await _appBll.SaveChangesAsync();

        return CreatedAtAction("GetAssignment", new { id = assignment.Id }, assignment);
    }

    // DELETE: api/<version>/Assignment/<id>
    /// <summary>Delete assignment</summary>
    /// <param name="id">ID of assignment</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssignment(Guid id)
    {
        if (!_appBll.Assignments.IsUserAssignment(id, User.GetUserId()))
            return Unauthorized();

        var assignment = _appBll.Assignments.Remove(id);
        
        var order = await _appBll.Orders.FirstOrDefaultAsync(assignment.OrderId);
        if (order == null)
            return NotFound();
        
        var client = await _appBll.Clients.FirstOrDefaultAsync(order.ClientId);
        if (client == null)
            return NotFound();

        order.TotalTasks--;
        client.TotalTasks--;
        
        _appBll.Orders.Update(order);
        _appBll.Clients.Update(client);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }
}