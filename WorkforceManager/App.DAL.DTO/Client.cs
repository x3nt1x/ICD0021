using Base.Domain;

namespace App.DAL.DTO;

public class Client : DomainEntityId
{
    public string Name { get; set; } = default!;

    public int TotalOrders { get; set; }
    public int TotalTasks { get; set; }

    public ICollection<Contact>? Contacts { get; set; }
    public ICollection<Order>? Orders { get; set; }
}