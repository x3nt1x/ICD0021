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
/// API endpoint for managing comments
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentController : ControllerBase
{
    private readonly IAppBll _appBll;
    private readonly CommentMapper _mapper;

    public CommentController(IAppBll appBll, AutoMapper.IMapper mapper)
    {
        _appBll = appBll;
        _mapper = new CommentMapper(mapper);
    }

    // GET: api/<version>/Comment
    /// <summary>Get all comments</summary>
    /// <returns>List of comments</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Comment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
    {
        return (await _appBll.Comments.AllAsync()).Select(comment => _mapper.Map(comment)).ToList()!;
    }

    // GET: api/<version>/Comment/<id>
    /// <summary>Get single comment</summary>
    /// <param name="id">ID of comment</param>
    /// <returns>Requested comment</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK)]
    public async Task<ActionResult<Comment>> GetComment(Guid id)
    {
        var comment = await _appBll.Comments.FirstOrDefaultAsync(id);
        if (comment == null)
            return NotFound();

        return _mapper.Map(comment)!;
    }

    // PUT: api/<version>/Comment/<id>
    /// <summary>Update comment</summary>
    /// <param name="id">ID of comment</param>
    /// <param name="comment">New data to insert</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutComment(Guid id, Comment comment)
    {
        if (id != comment.Id)
            return BadRequest();
        
        if (!_appBll.Comments.IsUserComment(id, User.GetUserId()))
            return Unauthorized();
        
        comment.Date = DateTime.Now.ToUniversalTime();
        comment.AppUserId = User.GetUserId();

        _appBll.Comments.Update(_mapper.Map(comment)!);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/<version>/Comment
    /// <summary>Create comment</summary>
    /// <param name="comment">New comment data</param>
    /// <returns>Created comment</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    public async Task<ActionResult<Comment>> PostComment(Comment comment)
    {
        comment.Id = new Guid();
        comment.Date = DateTime.Now.ToUniversalTime();
        comment.AppUserId = User.GetUserId();

        _appBll.Comments.Add(_mapper.Map(comment)!);

        await _appBll.SaveChangesAsync();

        return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
    }

    // DELETE: api/<version>/Comment/<id>
    /// <summary>Delete comment</summary>
    /// <param name="id">ID of comment</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        if (!_appBll.Comments.IsUserComment(id, User.GetUserId()))
            return Unauthorized();
        
        _appBll.Comments.Remove(id);

        await _appBll.SaveChangesAsync();

        return NoContent();
    }
}