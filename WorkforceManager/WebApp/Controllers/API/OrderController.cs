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
/// API endpoint for managing orders
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OrderController : ControllerBase
{
    private readonly IAppBll _appBll;
    private readonly OrderMapper _mapper;
    private readonly AssignmentMapper _assignmentMapper;

    public OrderController(IAppBll appBll, AutoMapper.IMapper mapper)
    {
        _appBll = appBll;
        _mapper = new OrderMapper(mapper);
        _assignmentMapper = new AssignmentMapper(mapper);
    }

    // GET: api/<version>/Order
    /// <summary>Get all orders</summary>
    /// <returns>List of orders</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return (await _appBll.Orders.AllAsync()).Select(order => _mapper.MapWithClient(order)).ToList()!;
    }

    // GET: api/<version>/Order/<id>
    /// <summary>Get single order</summary>
    /// <param name="id">ID of order</param>
    /// <returns>Requested order</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<ActionResult<Order>> GetOrder(Guid? id)
    {
        if (id == null)
            return NotFound();

        var order = await _appBll.Orders.FirstOrDefaultAsync(id.Value);
        if (order == null)
            return NotFound();

        return _mapper.Map(order)!;
    }
    
    // GET: api/<version>/Order/<id>/Assignments
    /// <summary>Get all order assignments</summary>
    /// <param name="id">ID of order</param>
    /// <returns>List of order assignments</returns>
    [HttpGet("{id}/Assignments")]
    [ProducesResponseType(typeof(IEnumerable<Assignment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Assignment>>> GetOrderAssignments(Guid id)
    {
        return (await _appBll.Assignments.GetOrderAssignmentsAsync(id))
            .Select(assignment => _assignmentMapper.Map(assignment))
            .ToList()!;
    }

    // PUT: api/<version>/Order/<id>
    /// <summary>Update order</summary>
    /// <param name="id">ID of order</param>
    /// <param name="order">New data to insert</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutOrder(Guid id, Order order)
    {
        if (id != order.Id)
            return BadRequest();

        _appBll.Orders.Update(_mapper.Map(order)!);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/<version>/Order
    /// <summary>Create order</summary>
    /// <param name="order">New order data</param>
    /// <returns>Created order</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> PostOrder(Order order)
    {
        order.Id = new Guid();
        order.Start = DateOnly.FromDateTime(DateTime.Now);

        _appBll.Orders.Add(_mapper.Map(order)!);

        var client = await _appBll.Clients.FirstOrDefaultAsync(order.ClientId);
        if (client == null)
            return NotFound();

        client.TotalOrders++;
        _appBll.Clients.Update(client);

        await _appBll.SaveChangesAsync();

        return CreatedAtAction("GetOrder", new { id = order.Id }, order);
    }

    // DELETE: api/<version>/Order/<id>
    /// <summary>Delete order</summary>
    /// <param name="id">ID of order</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var order = _appBll.Orders.Remove(id);

        var client = await _appBll.Clients.FirstOrDefaultAsync(order.ClientId);
        if (client == null)
            return NotFound();

        client.TotalOrders--;
        client.TotalTasks -= order.TotalTasks;
        
        _appBll.Clients.Update(client);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }
}