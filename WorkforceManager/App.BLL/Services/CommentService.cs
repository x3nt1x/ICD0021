using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CommentService : BaseEntityService<BLL.DTO.Comment, DAL.DTO.Comment, ICommentRepository>, ICommentService
{
    public CommentService(ICommentRepository repository, IMapper<BLL.DTO.Comment, DAL.DTO.Comment> mapper) : base(repository, mapper)
    {
    }

    public bool IsUserComment(Guid id, Guid userId)
    {
        return Repository.IsUserComment(id, userId);
    }

    public async Task<IEnumerable<BLL.DTO.Comment>> GetAssignmentCommentsAsync(Guid assignmentId)
    {
        return (await Repository.GetAssignmentCommentsAsync(assignmentId))
            .Select(comment => Mapper.Map(comment))
            .ToList()!;
    }
}