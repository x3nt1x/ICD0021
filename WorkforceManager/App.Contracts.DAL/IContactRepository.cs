using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IContactRepository : IBaseRepository<Contact>
{
    public Task<Contact?> FindAsync(Guid id, Guid userId);
    public Task<IEnumerable<Contact>> AllAsync(Guid userId);
    public Task<IEnumerable<Contact>> GetClientContactsAsync(Guid clientId);
}