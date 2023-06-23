using App.Contracts.DAL;
using App.DAL.Mappers;
using App.DAL.Repositories;
using Base.DAL.EF;

namespace App.DAL;

public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUow
{
    private readonly AutoMapper.IMapper _mapper;

    public AppUnitOfWork(AppDbContext dbContext, AutoMapper.IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private IAssignmentRepository? _assignments;
    private IClientRepository? _clients;
    private ICommentRepository? _comments;
    private IContactRepository? _contacts;
    private IOrderRepository? _orders;
    private IWorkerRepository? _workers;

    public IAssignmentRepository Assignments =>
        _assignments ??= new AssignmentRepository(DbContext, new AssignmentMapper(_mapper));

    public IClientRepository Clients =>
        _clients ??= new ClientRepository(DbContext, new ClientMapper(_mapper));

    public ICommentRepository Comments =>
        _comments ??= new CommentRepository(DbContext, new CommentMapper(_mapper));

    public IContactRepository Contacts =>
        _contacts ??= new ContactRepository(DbContext, new ContactMapper(_mapper));

    public IOrderRepository Orders =>
        _orders ??= new OrderRepository(DbContext, new OrderMapper(_mapper));

    public IWorkerRepository Workers =>
        _workers ??= new WorkerRepository(DbContext, new WorkerMapper(_mapper));
}