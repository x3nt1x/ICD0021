using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class AssignmentService : BaseEntityService<BLL.DTO.Assignment, DAL.DTO.Assignment, IAssignmentRepository>, IAssignmentService
{
    public AssignmentService(IAssignmentRepository repository, IMapper<BLL.DTO.Assignment, DAL.DTO.Assignment> mapper) : base(repository, mapper)
    {
    }

    public bool IsUserAssignment(Guid id, Guid userId)
    {
        return Repository.IsUserAssignment(id, userId);
    }

    public async Task<Assignment?> GetAssignmentAsync(Guid id)
    {
        return Mapper.Map(await Repository.GetAssignmentAsync(id));
    }

    public async Task<IEnumerable<Assignment>> GetUserAssignmentsAsync(Guid userId)
    {
        return (await Repository.GetUserAssignmentsAsync(userId))
            .Select(assignment => Mapper.Map(assignment))
            .ToList()!;
    }

    public async Task<IEnumerable<Assignment>> GetOrderAssignmentsAsync(Guid orderId)
    {
        return (await Repository.GetOrderAssignmentsAsync(orderId))
            .Select(assignment => Mapper.Map(assignment))
            .ToList()!;
    }

    public async Task<IEnumerable<Assignment>> GetClientAssignmentsAsync(Guid clientId)
    {
        return (await Repository.GetClientAssignmentsAsync(clientId))
            .Select(assignment => Mapper.Map(assignment))
            .ToList()!;
    }
}