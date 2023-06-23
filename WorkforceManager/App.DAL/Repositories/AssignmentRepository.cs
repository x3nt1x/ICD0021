using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories;

public class AssignmentRepository : BaseEntityRepository<DAL.DTO.Assignment, Domain.Assignment, AppDbContext>, IAssignmentRepository
{
    public AssignmentRepository(AppDbContext dbContext, IMapper<DAL.DTO.Assignment, Domain.Assignment> mapper) : base(dbContext, mapper)
    {
    }
    
    public bool IsUserAssignment(Guid id, Guid userId)
    {
        return RepositoryDbSet
            .AsNoTracking()
            .Any(assignment => assignment.Id == id && assignment.Workers!.Any(worker => worker.AppUserId == userId));
    }
    
    public async Task<Assignment?> GetAssignmentAsync(Guid id)
    {
        return RepoMapper.Map(await RepositoryDbSet
            .AsNoTracking()
            .Where(assignment => assignment.Id == id)
            .FirstOrDefaultAsync());
    }

    public async Task<IEnumerable<Assignment>> GetUserAssignmentsAsync(Guid userId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Where(assignment => assignment.Workers!.Any(worker => worker.AppUserId == userId))
            .Select(assignment => RepoMapper.Map(assignment))
            .ToListAsync())!;
    }

    public async Task<IEnumerable<Assignment>> GetOrderAssignmentsAsync(Guid orderId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Where(assignment => assignment.Order!.Id.Equals(orderId))
            .Select(assignment => RepoMapper.Map(assignment))
            .ToListAsync())!;
    }

    public async Task<IEnumerable<Assignment>> GetClientAssignmentsAsync(Guid clientId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Where(assignment => assignment.Order!.ClientId.Equals(clientId))
            .Select(assignment => RepoMapper.Map(assignment))
            .ToListAsync())!;
    }
}