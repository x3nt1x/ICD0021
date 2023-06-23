using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;

namespace App.BLL;

public class AppBll : BaseBll<IAppUow>, IAppBll
{
    private readonly AutoMapper.IMapper _mapper;

    public AppBll(IAppUow uow, AutoMapper.IMapper mapper) : base(uow)
    {
        _mapper = mapper;
    }

    private IAssignmentService? _assignments;
    private IClientService? _clients;
    private ICommentService? _comments;
    private IContactService? _contacts;
    private IOrderService? _orders;
    private IWorkerService? _workers;

    public IAssignmentService Assignments =>
        _assignments ??= new AssignmentService(Uow.Assignments, new AssignmentMapper(_mapper));

    public IClientService Clients =>
        _clients ??= new ClientService(Uow.Clients, new ClientMapper(_mapper));

    public ICommentService Comments =>
        _comments ??= new CommentService(Uow.Comments, new CommentMapper(_mapper));

    public IContactService Contacts =>
        _contacts ??= new ContactService(Uow.Contacts, new ContactMapper(_mapper));

    public IOrderService Orders =>
        _orders ??= new OrderService(Uow.Orders, new OrderMapper(_mapper));

    public IWorkerService Workers =>
        _workers ??= new WorkerService(Uow.Workers, new WorkerMapper(_mapper));
}