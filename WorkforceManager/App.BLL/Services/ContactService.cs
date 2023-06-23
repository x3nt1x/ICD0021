using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ContactService : BaseEntityService<BLL.DTO.Contact, DAL.DTO.Contact, IContactRepository>, IContactService
{
    public ContactService(IContactRepository repository, IMapper<BLL.DTO.Contact, DAL.DTO.Contact> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Contact>> GetClientCommentsAsync(Guid clientId)
    {
        return (await Repository.GetClientContactsAsync(clientId))
            .Select(contact => Mapper.Map(contact))
            .ToList()!;
    }
}