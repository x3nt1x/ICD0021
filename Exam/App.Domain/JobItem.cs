using Base.Domain;

namespace App.Domain;

public class JobItem : DomainEntityId
{
    public int Quantity { get; set; }
    
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    
    public Guid JobId { get; set; }
    public Job? Job { get; set; }
}