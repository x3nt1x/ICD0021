using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Worker : DomainEntityId
{
    public Guid AssignmentId { get; set; }
    public Guid AppUserId { get; set; }
    
    [MinLength(1)] [MaxLength(32)]
    public string? FirstName { get; set; }
    
    [MinLength(1)] [MaxLength(32)]
    public string? LastName { get; set; }
}