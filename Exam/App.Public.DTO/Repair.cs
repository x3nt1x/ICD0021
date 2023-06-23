using Base.Domain;

namespace App.Public.DTO;

public class Repair : DomainEntityId
{
    public TimeOnly Schedule { get; set; }
    public TimeOnly TotalTime { get; set; }
    public double TotalPrice { get; set; }
    public int TotalJobs { get; set; }
    
    public ICollection<RepairJob>? RepairJobs { get; set; }
}