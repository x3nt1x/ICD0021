using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IClientRepository : IBaseRepository<Client>
{
    public Task<Client?> FindAsync(Guid id, Guid userId);
    public Task<IEnumerable<Client>> AllAsync(Guid userId);
}