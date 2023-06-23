using Base.Domain;

namespace App.Public.DTO;

public class JobItem : DomainEntityId
{
    public int Quantity { get; set; }
    
    public Guid ItemId { get; set; }
    public Guid JobId { get; set; }
}