using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    public bool IsUserAssignment(Guid id, Guid userId);
    public Task<Assignment?> GetAssignmentAsync(Guid id);
    public Task<IEnumerable<Assignment>> GetUserAssignmentsAsync(Guid userId);
    public Task<IEnumerable<Assignment>> GetOrderAssignmentsAsync(Guid orderId);
    public Task<IEnumerable<Assignment>> GetClientAssignmentsAsync(Guid clientId);
}