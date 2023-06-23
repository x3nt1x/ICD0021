using App.Domain;
using Base.Domain;

namespace App.Public.DTO;

public class RepairJob : DomainEntityId
{
    public EStatus Status { get; set; } = EStatus.Pending;
    
    public Guid JobId { get; set; }
    public Guid RepairId { get; set; }
}