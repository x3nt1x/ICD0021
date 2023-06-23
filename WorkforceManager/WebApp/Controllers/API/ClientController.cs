using System.Net.Mime;
using App.Contracts.BLL;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers.API;

/// <summary>
/// API endpoint for managing clients
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClientController : ControllerBase
{
    private readonly IAppBll _appBll;
    private readonly ClientMapper _mapper;
    private readonly OrderMapper _orderMapper;
    private readonly ContactMapper _contactMapper;
    private readonly AssignmentMapper _assignmentMapper;

    public ClientController(IAppBll appBll, AutoMapper.IMapper mapper)
    {
        _appBll = appBll;
        _mapper = new ClientMapper(mapper);
        _orderMapper = new OrderMapper(mapper);
        _contactMapper = new ContactMapper(mapper);
        _assignmentMapper = new AssignmentMapper(mapper);
    }

    // GET: api/<version>/Client
    /// <summary>Get all clients</summary>
    /// <returns>List of clients</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        return (await _appBll.Clients.AllAsync()).Select(client => _mapper.Map(client)).ToList()!;
    }

    // GET: api/<version>/Client/<id>
    /// <summary>Get single client</summary>
    /// <param name="id">ID of client</param>
    /// <returns>Requested client</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
    public async Task<ActionResult<Client>> GetClient(Guid? id)
    {
        if (id == null)
            return NotFound();

        var client = await _appBll.Clients.FirstOrDefaultAsync(id.Value);
        if (client == null)
            return NotFound();

        return _mapper.Map(client)!;
    }

    // GET: api/<version>/Client/<id>/Orders
    /// <summary>Get all client orders</summary>
    /// <param name="id">ID of client</param>
    /// <returns>List of client orders</returns>
    [HttpGet("{id}/Orders")]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Order>>> GetClientOrders(Guid id)
    {
        return (await _appBll.Orders.GetClientOrdersAsync(id))
            .Select(order => _orderMapper.Map(order))
            .ToList()!;
    }
    
    // GET: api/<version>/Client/<id>/Contacts
    /// <summary>Get all client contacts</summary>
    /// <param name="id">ID of client</param>
    /// <returns>List of client contacts</returns>
    [HttpGet("{id}/Contacts")]
    [ProducesResponseType(typeof(IEnumerable<Contact>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Contact>>> GetClientContacts(Guid id)
    {
        return (await _appBll.Contacts.GetClientCommentsAsync(id))
            .Select(contact => _contactMapper.Map(contact))
            .ToList()!;
    }
    
    // GET: api/<version>/Client/<id>/Assignments
    /// <summary>Get all client assignments</summary>
    /// <param name="id">ID of client</param>
    /// <returns>List of client assignments</returns>
    [HttpGet("{id}/Assignments")]
    [ProducesResponseType(typeof(IEnumerable<Assignment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Assignment>>> GetClientAssignments(Guid id)
    {
        return (await _appBll.Assignments.GetClientAssignmentsAsync(id))
            .Select(assignment => _assignmentMapper.Map(assignment))
            .ToList()!;
    }

    // PUT: api/<version>/Client/<id>
    /// <summary>Update client</summary>
    /// <param name="id">ID of client</param>
    /// <param name="client">New data to insert</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutClient(Guid id, Client client)
    {
        if (id != client.Id)
            return BadRequest();

        _appBll.Clients.Update(_mapper.Map(client)!);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/<version>/Client
    /// <summary>Create client</summary>
    /// <param name="client">New client data</param>
    /// <returns>Created client</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Client), StatusCodes.Status201Created)]
    public async Task<ActionResult<Client>> PostClient(Client client)
    {
        client.Id = new Guid();

        var newClient = _appBll.Clients.Add(_mapper.Map(client)!);

        await _appBll.SaveChangesAsync();

        return CreatedAtAction("GetClient", new { id = newClient.Id }, newClient);
    }

    // DELETE: api/<version>/Client/<id>
    /// <summary>Delete client</summary>
    /// <param name="id">ID of client</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        _appBll.Clients.Remove(id);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }
}