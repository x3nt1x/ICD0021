using App.Domain.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class Worker : DomainEntityId
{
    public Guid AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}