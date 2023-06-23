using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IAssignmentService : IEntityService<Assignment>
{
    public bool IsUserAssignment(Guid id, Guid userId);
    public Task<Assignment?> GetAssignmentAsync(Guid id);
    public Task<IEnumerable<Assignment>> GetUserAssignmentsAsync(Guid userId);
    public Task<IEnumerable<Assignment>> GetOrderAssignmentsAsync(Guid orderId);
    public Task<IEnumerable<Assignment>> GetClientAssignmentsAsync(Guid clientId);
}