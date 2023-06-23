namespace App.Public.DTO.Identity;

public class RefreshTokenDto
{
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}