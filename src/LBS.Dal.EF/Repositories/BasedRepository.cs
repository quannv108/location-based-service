using LBS.Dal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LBS.Dal.EF.Repositories;

public abstract class BasedRepository<T> : IRepository<T> where T : class
{
    protected readonly LbsDbContext _lbsDbContext;

    protected BasedRepository(LbsDbContext lbsDbContext)
    {
        _lbsDbContext = lbsDbContext;
    }

    public IQueryable<T> Queryable()
    {
        return _lbsDbContext.Set<T>();
    }

    public async Task<T> CreateAsync(T newEntity)
    {
        var entityEntry = await _lbsDbContext.Set<T>().AddAsync(newEntity);
        return entityEntry.Entity;
    }

    public Task<long> CountAsync()
    {
        return Queryable().LongCountAsync();
    }

    public Task SaveChangeAsync()
    {
        return _lbsDbContext.SaveChangesAsync();
    }
}