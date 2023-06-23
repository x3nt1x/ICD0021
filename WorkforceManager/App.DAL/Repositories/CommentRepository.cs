using App.Contracts.DAL;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories;

public class CommentRepository : BaseEntityRepository<DAL.DTO.Comment, Domain.Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext, IMapper<DAL.DTO.Comment, Domain.Comment> mapper) : base(dbContext, mapper)
    {
    }

    public bool IsUserComment(Guid id, Guid userId)
    {
        return RepositoryDbSet
            .AsNoTracking()
            .Any(comment => comment.Id == id && comment.AppUserId == userId);
    }

    public async Task<DAL.DTO.Comment?> FindAsync(Guid id, Guid userId)
    {
        return RepoMapper.Map(await RepositoryDbSet.Where(comment => comment.Id == id).FirstOrDefaultAsync());
    }

    public async Task<IEnumerable<DAL.DTO.Comment>> AllAsync(Guid userId)
    {
        return (await RepositoryDbSet.Select(comment => RepoMapper.Map(comment)).ToListAsync())!;
    }

    public async Task<IEnumerable<DAL.DTO.Comment>> GetAssignmentCommentsAsync(Guid assignmentId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Include(comment => comment.AppUser)
            .Where(comment => comment.AssignmentId.Equals(assignmentId))
            .Select(comment => RepoMapper.Map(comment))
            .ToListAsync())!;
    }
}