using App.Domain.Identity;
using Base.Domain;

namespace App.DAL.DTO;

public class Comment : DomainEntityId
{
    public string Content { get; set; } = default!;

    public DateTime Date { get; set; }
    
    public Guid AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}