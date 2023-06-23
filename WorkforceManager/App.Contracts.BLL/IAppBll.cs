using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBll : IBaseBll
{
    IAssignmentService Assignments { get; }
    IClientService Clients { get; }
    ICommentService Comments { get; }
    IContactService Contacts { get; }
    IOrderService Orders { get; }
    IWorkerService Workers { get; }
}