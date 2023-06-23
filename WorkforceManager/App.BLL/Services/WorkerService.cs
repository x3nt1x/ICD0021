using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class WorkerService : BaseEntityService<BLL.DTO.Worker, DAL.DTO.Worker, IWorkerRepository>, IWorkerService
{
    public WorkerService(IWorkerRepository repository, IMapper<BLL.DTO.Worker, DAL.DTO.Worker> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Worker>> GetAssignmentWorkersAsync(Guid assignmentId)
    {
        return (await Repository.GetAssignmentWorkersAsync(assignmentId))
            .Select(worker => Mapper.Map(worker))
            .ToList()!;
    }
}