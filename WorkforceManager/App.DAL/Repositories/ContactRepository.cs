using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories;

public class ContactRepository : BaseEntityRepository<DAL.DTO.Contact, Domain.Contact, AppDbContext>, IContactRepository
{
    public ContactRepository(AppDbContext dbContext, IMapper<DAL.DTO.Contact, Domain.Contact> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Contact?> FindAsync(Guid id, Guid userId)
    {
        return RepoMapper.Map(await RepositoryDbSet.Where(contact => contact.Id == id).FirstOrDefaultAsync());
    }

    public async Task<IEnumerable<Contact>> AllAsync(Guid userId)
    {
        return (await RepositoryDbSet.ToListAsync()).Select(contact => RepoMapper.Map(contact))!;
    }

    public async Task<IEnumerable<Contact>> GetClientContactsAsync(Guid clientId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Where(contact => contact.ClientId.Equals(clientId))
            .Select(contact => RepoMapper.Map(contact))
            .ToListAsync())!;
    }
}