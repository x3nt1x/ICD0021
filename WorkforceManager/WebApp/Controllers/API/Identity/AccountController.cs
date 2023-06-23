using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using App.DAL;
using App.Domain.Identity;
using App.Public.DTO.Mappers;
using App.Public.DTO.v1.Identity;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers.API.Identity;

/// <summary>
/// API endpoint for managing user accounts
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AppUserMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly UserManager<App.Domain.Identity.AppUser> _userManager;
    private readonly SignInManager<App.Domain.Identity.AppUser> _signInManager;

    private readonly Random _random = new();

    public AccountController(AppDbContext context, UserManager<App.Domain.Identity.AppUser> userManager,
                             IConfiguration configuration, SignInManager<App.Domain.Identity.AppUser> signInManager,
                             AutoMapper.IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = new AppUserMapper(mapper);
    }
    
    // GET: api/<version>/identity/Account/GetUsers
    /// <summary>Get all registered users</summary>
    /// <returns>List of registered users</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Identity.AppUser>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Identity.AppUser>>> GetUsers()
    {
        return (await _context.Users.ToListAsync()).Select(user => _mapper.Map(user)).ToList()!;
    }
    
    // POST: api/<version>/identity/Account/Register
    /// <summary>Register new user</summary>
    /// <param name="data">New user data</param>
    /// <returns>JWT Response</returns>
    [HttpPost]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JwtResponse>> Register([FromBody] Register data)
    {
        // user is already registered
        var appUser = await _userManager.FindByEmailAsync(data.Email);
        if (appUser != null)
            return Problem($"User with email {data.Email} is already registered", null, StatusCodes.Status400BadRequest);

        // register user
        var refreshToken = new AppRefreshToken();

        appUser = new App.Domain.Identity.AppUser
        {
            Email = data.Email,
            UserName = data.Email,
            FirstName = data.Firstname,
            LastName = data.Lastname,
            AppRefreshTokens = new List<AppRefreshToken> { refreshToken }
        };

        refreshToken.AppUser = appUser;

        var result = await _userManager.CreateAsync(appUser, data.Password);
        if (!result.Succeeded)
            return Problem(result.Errors.First().Description, null, StatusCodes.Status400BadRequest);

        // save the user full name into claims
        result = await _userManager.AddClaimsAsync(appUser, new List<Claim>
        {
            new(ClaimTypes.GivenName, appUser.FirstName),
            new(ClaimTypes.Surname, appUser.LastName)
        });

        if (!result.Succeeded)
            return Problem(result.Errors.First().Description, null, StatusCodes.Status400BadRequest);

        // get full user from system with fixed data
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
            return Problem($"User with email {data.Email} is not found after registration", null, StatusCodes.Status400BadRequest);

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        // generate jwt
        var JWT = IdentityHelpers.GenerateJWT
        (
            claimsPrincipal.Claims,
            _configuration.GetValue<string>("JWT:Key")!,
            _configuration.GetValue<string>("JWT:Issuer")!,
            _configuration.GetValue<string>("JWT:Audience")!,
            _configuration.GetValue<int>("JWT:ExpireSeconds")!
        );

        var response = new JwtResponse
        {
            JWT = JWT,
            RefreshToken = refreshToken.RefreshToken
        };

        return Ok(response);
    }
    
    // POST: api/<version>/identity/Account/LogIn
    /// <summary>Login user</summary>
    /// <param name="data">User login data</param>
    /// <returns>JWT and refresh token</returns>
    [HttpPost]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] Login data)
    {
        // verify username
        var appUser = await _userManager.FindByEmailAsync(data.Email);
        if (appUser == null)
        {
            await Task.Delay(_random.Next(100, 1000));
            
            return Problem("Username or password incorrect", null, StatusCodes.Status404NotFound);
        }

        // verify password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, data.Password, false);
        if (!result.Succeeded)
        {
            await Task.Delay(_random.Next(100, 1000));

            return Problem("Username or password incorrect", null, StatusCodes.Status404NotFound);
        }

        appUser.AppRefreshTokens = await _context
            .Entry(appUser)
            .Collection(user => user.AppRefreshTokens!)
            .Query()
            .Where(token => token.AppUserId == appUser.Id)
            .ToListAsync();

        // remove expired tokens
        foreach (var userRefreshToken in appUser.AppRefreshTokens)
        {
            if (userRefreshToken.Expiration < DateTime.UtcNow && 
               (userRefreshToken.PreviousExpiration == null || userRefreshToken.PreviousExpiration < DateTime.UtcNow))
            {
                _context.AppRefreshTokens.Remove(userRefreshToken);
            }
        }
        
        // save the user full name into claims
        await _userManager.AddClaimsAsync(appUser, new List<Claim>
        {
            new(ClaimTypes.GivenName, appUser.FirstName),
            new(ClaimTypes.Surname, appUser.LastName)
        });

        var newRefreshToken = new AppRefreshToken
        {
            AppUserId = appUser.Id
        };
        
        _context.AppRefreshTokens.Add(newRefreshToken);
        
        await _context.SaveChangesAsync();

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        // generate jwt
        var JWT = IdentityHelpers.GenerateJWT
        (
            claimsPrincipal.Claims, 
            _configuration.GetValue<string>("JWT:Key")!,
            _configuration.GetValue<string>("JWT:Issuer")!,
            _configuration.GetValue<string>("JWT:Audience")!,
            _configuration.GetValue<int>("JWT:ExpireSeconds")!
        );

        var response = new JwtResponse
        {
            JWT = JWT,
            RefreshToken = newRefreshToken.RefreshToken
        };

        return Ok(response);
    }
    
    // POST: api/<version>/identity/Account/Logout
    /// <summary>Logout user</summary>
    /// <param name="data">User data</param>
    [HttpPost]
    [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Logout([FromBody] Logout data)
    {
        var appUser = await _context.Users.Where(user => user.Id == User.GetUserId()).SingleOrDefaultAsync();
        if (appUser == null)
            return Problem("User not found", null, StatusCodes.Status404NotFound);

        await _context
            .Entry(appUser)
            .Collection(user => user.AppRefreshTokens!)
            .Query()
            .Where(token => token.RefreshToken == data.RefreshToken || token.PreviousRefreshToken == data.RefreshToken)
            .ToListAsync();

        foreach (var appRefreshToken in appUser.AppRefreshTokens!) 
            _context.AppRefreshTokens.Remove(appRefreshToken);

        await _context.SaveChangesAsync();

        return Ok();
    }
    
    // POST: api/<version>/identity/Account/RefreshToken
    /// <summary>Refresh JWT</summary>
    /// <param name="refreshTokenDTO">Old JWT and refresh token</param>
    [HttpPost]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDTO)
    {
        JwtSecurityToken jwtToken;
        
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenDTO.JWT);
            
            if (jwtToken == null)
                return Problem("No token", null, StatusCodes.Status400BadRequest);
        }
        catch (Exception e)
        {
            return Problem($"Cant parse the token, {e.Message}", null, StatusCodes.Status400BadRequest);
        }

        var key = _configuration.GetValue<string>("JWT:Key")!;
        var issuer = _configuration.GetValue<string>("JWT:Issuer")!;
        var audience = _configuration.GetValue<string>("JWT:Audience")!;
        var expires = _configuration.GetValue<int>("JWT:ExpireSeconds")!;
        
        if (!IdentityHelpers.ValidateToken(refreshTokenDTO.JWT, key, issuer, audience))
            return Problem("JWT validation error", null, StatusCodes.Status400BadRequest);

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
            return Problem("No email in JWT", null, StatusCodes.Status400BadRequest);

        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
            return Problem($"User with email {userEmail} not found", null, StatusCodes.Status404NotFound);

        // load and compare refresh tokens
        await _context
            .Entry(appUser)
            .Collection(user => user.AppRefreshTokens!)
            .Query()
            .Where(token =>
                (token.RefreshToken == refreshTokenDTO.RefreshToken && token.Expiration > DateTime.UtcNow) ||
                (token.PreviousRefreshToken == refreshTokenDTO.RefreshToken && token.PreviousExpiration > DateTime.UtcNow))
            .ToListAsync();

        if (appUser.AppRefreshTokens == null)
            return Problem("RefreshTokens collection is null", null, StatusCodes.Status400BadRequest);

        if (appUser.AppRefreshTokens.Count == 0)
            return Problem("RefreshTokens collection is empty", null, StatusCodes.Status400BadRequest);

        if (appUser.AppRefreshTokens.Count != 1)
            return Problem("More than one valid refresh token found", null, StatusCodes.Status400BadRequest);

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        // generate jwt
        var JWT = IdentityHelpers.GenerateJWT
        (
            claimsPrincipal.Claims,
            key,
            issuer,
            audience,
            expires
        );

        // make new refresh token, keep old one still valid for some time
        var refreshToken = appUser.AppRefreshTokens.First();
        if (refreshToken.RefreshToken == refreshTokenDTO.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpiration = DateTime.UtcNow.AddMinutes(1);

            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.Expiration = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }

        var response = new JwtResponse
        {
            JWT = JWT,
            RefreshToken = refreshToken.RefreshToken
        };

        return Ok(response);
    }
}