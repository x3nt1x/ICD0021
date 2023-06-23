using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Comment : DomainEntityId
{
    public Guid? AssignmentId { get; set; }
    public Guid? AppUserId { get; set; }

    [MinLength(1)] [MaxLength(512)]
    public string Content { get; set; } = default!;

    [MinLength(1)] [MaxLength(32)]
    public string? FirstName { get; set; }
    
    [MinLength(1)] [MaxLength(32)]
    public string? LastName { get; set; }
    
    public DateTime Date { get; set; }
}