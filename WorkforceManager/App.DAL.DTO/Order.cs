using Base.Domain;

namespace App.DAL.DTO;

public class Order : DomainEntityId
{
    public string Name { get; set; } = default!;

    public int TotalTasks { get; set; }
    
    public DateOnly Start { get; set; }
    public DateOnly? End { get; set; }
    
    public Guid ClientId { get; set; }
    public Client? Client { get; set; }
    
    public ICollection<Assignment>? Assignments { get; set; }
}