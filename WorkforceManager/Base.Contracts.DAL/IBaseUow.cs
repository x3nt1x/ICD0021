namespace Base.Contracts.DAL;

public interface IBaseUow
{
    Task<int> SaveChangesAsync();
}