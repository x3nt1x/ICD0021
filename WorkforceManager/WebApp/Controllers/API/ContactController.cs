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
/// API endpoint for managing contacts
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ContactController : ControllerBase
{
    private readonly IAppBll _appBll;
    private readonly ContactMapper _mapper;

    public ContactController(IAppBll appBll, AutoMapper.IMapper mapper)
    {
        _appBll = appBll;
        _mapper = new ContactMapper(mapper);
    }

    // GET: api/<version>/Contact
    /// <summary>Get all contacts</summary>
    /// <returns>List of contacts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Contact>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    {
        return (await _appBll.Contacts.AllAsync()).Select(contact => _mapper.Map(contact)).ToList()!;
    }

    // GET: api/<version>/Contact/<id>
    /// <summary>Get single contact</summary>
    /// <param name="id">ID of contact</param>
    /// <returns>Requested contact</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    public async Task<ActionResult<Contact>> GetContact(Guid? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _appBll.Contacts.FirstOrDefaultAsync(id.Value);
        if (contact == null)
            return NotFound();

        return _mapper.Map(contact)!;
    }

    // PUT: api/<version>/Contact/<id>
    /// <summary>Update contact</summary>
    /// <param name="id">ID of contact</param>
    /// <param name="contact">New data to insert</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutContact(Guid id, Contact contact)
    {
        if (id != contact.Id)
            return BadRequest();

        _appBll.Contacts.Update(_mapper.Map(contact)!);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/<version>/Contact
    /// <summary>Create contact</summary>
    /// <param name="contact">New contact data</param>
    /// <returns>Created contact</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
    public async Task<ActionResult<Contact>> PostContact(Contact contact)
    {
        contact.Id = new Guid();

        _appBll.Contacts.Add(_mapper.Map(contact)!);

        await _appBll.SaveChangesAsync();

        return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
    }

    // DELETE: api/<version>/Contact/<id>
    /// <summary>Delete contact</summary>
    /// <param name="id">ID of contact</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        _appBll.Contacts.Remove(id);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }
}