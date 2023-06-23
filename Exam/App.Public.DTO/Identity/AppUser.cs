using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.Identity;

public class AppUser : DomainEntityId
{
    [MinLength(1)] [MaxLength(32)]
    public string FirstName { get; set; } = default!;

    [MinLength(1)] [MaxLength(32)]
    public string LastName { get; set; } = default!;
}