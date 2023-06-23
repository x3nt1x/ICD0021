using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ClientService : BaseEntityService<BLL.DTO.Client, DAL.DTO.Client, IClientRepository>, IClientService
{
    public ClientService(IClientRepository repository, IMapper<BLL.DTO.Client, DAL.DTO.Client> mapper) : base(repository, mapper)
    {
    }
}