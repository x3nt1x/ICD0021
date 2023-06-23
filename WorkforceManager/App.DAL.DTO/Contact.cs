using App.Domain.Enums;
using Base.Domain;

namespace App.DAL.DTO;

public class Contact : DomainEntityId
{
    public string Content { get; set; } = default!;
    public string? Info { get; set; }

    public EContactType Type { get; set; } = EContactType.Email;
    
    public Guid ClientId { get; set; }
    public Client? Client { get; set; }
}