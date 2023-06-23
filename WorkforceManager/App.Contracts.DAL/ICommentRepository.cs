using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ICommentRepository : IBaseRepository<Comment>
{
    public bool IsUserComment(Guid id, Guid userId);
    public Task<Comment?> FindAsync(Guid id, Guid userId);
    public Task<IEnumerable<Comment>> AllAsync(Guid userId);
    public Task<IEnumerable<Comment>> GetAssignmentCommentsAsync(Guid assignmentId);
}