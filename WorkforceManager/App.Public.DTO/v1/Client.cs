using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Client : DomainEntityId
{
    [MinLength(1)] [MaxLength(32)]
    public string Name { get; set; } = default!;

    public int TotalOrders { get; set; }
    public int TotalTasks { get; set; }
    
    public ICollection<Contact>? Contacts { get; set; }
    
    // Maybe?
    // public ICollection<Order>? Orders { get; set; }
}