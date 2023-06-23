using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;

namespace App.Domain;

public class Contact : DomainEntityId
{
    [MinLength(1)] [MaxLength(32)]
    public string Content { get; set; } = default!;
    
    [MinLength(1)] [MaxLength(32)]
    public string? Info { get; set; }

    public EContactType Type { get; set; } = EContactType.Email;
    
    public Guid ClientId { get; set; }
    public Client? Client { get; set; }
}