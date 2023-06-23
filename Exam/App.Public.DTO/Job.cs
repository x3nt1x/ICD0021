using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO;

public class Job : DomainEntityId
{
    [MinLength(1)] [MaxLength(32)]
    public string Name { get; set; } = default!;
    
    public TimeOnly Duration { get; set; }
    public double TotalPrice { get; set; }

    public ICollection<JobItem>? JobItems { get; set; }
    public ICollection<RepairJob>? RepairJobs { get; set; }
}