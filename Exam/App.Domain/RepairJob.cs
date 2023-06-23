using Base.Domain;

namespace App.Domain;

public class RepairJob : DomainEntityId
{
    public EStatus Status { get; set; } = EStatus.Pending;
    
    public Guid JobId { get; set; }
    public Job? Job { get; set; }
    
    public Guid RepairId { get; set; }
    public Repair? Repair { get; set; }
}