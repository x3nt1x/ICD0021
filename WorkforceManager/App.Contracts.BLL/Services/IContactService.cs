using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IContactService : IEntityService<Contact>
{
    public Task<IEnumerable<Contact>> GetClientCommentsAsync(Guid clientId);
}