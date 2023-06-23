using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IWorkerRepository : IBaseRepository<Worker>
{
    public Task<Worker?> FindAsync(Guid id, Guid userId);
    public Task<IEnumerable<Worker>> AllAsync(Guid userId);
    public Task<IEnumerable<Worker>> GetAssignmentWorkersAsync(Guid assignmentId);
}