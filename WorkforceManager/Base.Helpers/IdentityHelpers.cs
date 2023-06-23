using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Base.Helpers;

public static class IdentityHelpers
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (claim != null)
            return Guid.Parse(claim.Value);
                
        return Guid.Empty;
    }
    
    public static string GenerateJWT(IEnumerable<Claim> claims, string key, string issuer, string audience, int expireSeconds)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddSeconds(expireSeconds);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static bool ValidateToken(string token, string key, string issuer, string audience, bool ignoreExpiration = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters(key, issuer, audience);

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (SecurityTokenExpiredException)
        {
            return ignoreExpiration;
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private static TokenValidationParameters GetValidationParameters(string key, string issuer, string audience)
    {
        return new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    }
}