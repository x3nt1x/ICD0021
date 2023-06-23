using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories;

public class ClientRepository : BaseEntityRepository<DAL.DTO.Client, Domain.Client, AppDbContext>, IClientRepository
{
    public ClientRepository(AppDbContext dbContext, IMapper<DAL.DTO.Client, Domain.Client> mapper) : base(dbContext, mapper)
    {
    }

    public virtual async Task<Client?> FindAsync(Guid id, Guid userId)
    {
        return RepoMapper.Map(await RepositoryDbSet.Where(client => client.Id == id).FirstOrDefaultAsync());
    }

    public virtual async Task<IEnumerable<Client>> AllAsync(Guid userId)
    {
        var clients = await RepositoryDbSet.ToListAsync();
        
        return clients.Select(client => RepoMapper.Map(client))!;
    }
}