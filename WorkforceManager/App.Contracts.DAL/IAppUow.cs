using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUow : IBaseUow
{
    IAssignmentRepository Assignments { get; }
    IClientRepository Clients { get; }
    ICommentRepository Comments { get; }
    IContactRepository Contacts { get; }
    IOrderRepository Orders { get; }
    IWorkerRepository Workers { get; }
}