using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1.Identity;

public class Register
{
    [MinLength(1)] [MaxLength(128)]
    public string Email { get; set; } = default!;
    
    [MinLength(1)] [MaxLength(128)]
    public string Password { get; set; } = default!;
    
    [MinLength(1)] [MaxLength(32)]
    public string Firstname { get; set; } = default!;

    [MinLength(1)] [MaxLength(32)]
    public string Lastname { get; set; } = default!;
}