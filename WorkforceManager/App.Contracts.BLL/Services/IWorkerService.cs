using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IWorkerService : IEntityService<Worker>
{
    public Task<IEnumerable<Worker>> GetAssignmentWorkersAsync(Guid assignmentId);
}