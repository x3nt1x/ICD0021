using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Contact : DomainEntityId
{
    public Guid ClientId { get; set; }
    
    [MinLength(1)] [MaxLength(32)]
    public string Content { get; set; } = default!;
    
    [MinLength(1)] [MaxLength(32)]
    public string? Info { get; set; }

    public EContactType Type { get; set; } = EContactType.Email;
}