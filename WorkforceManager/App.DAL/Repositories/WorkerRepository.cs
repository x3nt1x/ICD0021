using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories;

public class WorkerRepository : BaseEntityRepository<DAL.DTO.Worker, Domain.Worker, AppDbContext>, IWorkerRepository
{
    public WorkerRepository(AppDbContext dbContext, IMapper<DAL.DTO.Worker, Domain.Worker> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Worker?> FindAsync(Guid id, Guid userId)
    {
        return RepoMapper.Map(await RepositoryDbSet.Where(worker => worker.Id == id).FirstOrDefaultAsync());
    }

    public async Task<IEnumerable<Worker>> AllAsync(Guid userId)
    {
        var workers = await RepositoryDbSet.ToListAsync();
        
        return workers.Select(worker => RepoMapper.Map(worker))!;
    }

    public async Task<IEnumerable<Worker>> GetAssignmentWorkersAsync(Guid assignmentId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Include(worker => worker.AppUser)
            .Where(worker => worker.AssignmentId.Equals(assignmentId))
            .Select(worker => RepoMapper.Map(worker))
            .ToListAsync())!;
    }
}