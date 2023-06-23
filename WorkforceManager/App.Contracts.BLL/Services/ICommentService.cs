using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICommentService : IEntityService<Comment>
{
     public bool IsUserComment(Guid id, Guid userId);
     public Task<IEnumerable<Comment>> GetAssignmentCommentsAsync(Guid assignmentId);
}