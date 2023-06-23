using App.Domain.Enums;
using Base.Domain;

namespace App.DAL.DTO;

public class Assignment : DomainEntityId
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ETaskPriority Priority { get; set; } = ETaskPriority.Low;
    public ETaskStatus Status { get; set; } = ETaskStatus.Pending;
    
    public DateOnly DueDate { get; set; }

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Worker>? Workers { get; set; }
}