using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Order : DomainEntityId
{
    public Guid ClientId { get; set; }
    
    [MinLength(1)] [MaxLength(32)]
    public string Name { get; set; } = default!;
    
    [MinLength(1)] [MaxLength(32)]
    public string? ClientName { get; set; }
    
    public int TotalTasks { get; set; }
    
    public DateOnly Start { get; set; }
    public DateOnly? End { get; set; }
    
    // Maybe?
    /*
    public ICollection<Assignment>? Assignments { get; set; }
    */
}