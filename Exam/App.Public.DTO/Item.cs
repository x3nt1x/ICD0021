using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Domain;

namespace App.Public.DTO;

public class Item : DomainEntityId
{
    [MinLength(1)] [MaxLength(32)]
    public string Name { get; set; } = default!;

    public ECategory Category { get; set; } = ECategory.Nut;
    public ELocation Location { get; set; } = ELocation.Drawer;

    public double Price { get; set; }
    
    public int Stock { get; set; }
    public int DefaultStock { get; set; }
}