namespace LBS.Dal.Repositories;

public interface IRepository<T>
{
    IQueryable<T> Queryable();
    Task<T> CreateAsync(T newEntity);
    Task<long> CountAsync();

    Task SaveChangeAsync();
}