using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Assignment : DomainEntityId
{
    public Guid OrderId { get; set; }
    
    [MinLength(1)] [MaxLength(32)]
    public string Title { get; set; } = default!;
    
    [MinLength(1)] [MaxLength(1024)]
    public string Description { get; set; } = default!;

    public ETaskPriority Priority { get; set; } = ETaskPriority.Low;
    public ETaskStatus Status { get; set; } = ETaskStatus.Pending;
    
    public DateOnly DueDate { get; set; }
    
    // Maybe?
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Worker>? Workers { get; set; }
}