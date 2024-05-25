using LBS.Dal.Models;
using LBS.Dal.Repositories;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace LBS.Dal.EF.Repositories;

public class NamedLocationRepository : BasedRepository<NamedLocation>, INamedLocationRepository
{
    public NamedLocationRepository(LbsDbContext lbsDbContext):base(lbsDbContext)
    {
    }
    
    public Task<List<NamedLocation>> GetNearbyLocationsAsync(Point point, double radius)
    {
        return Queryable()
            .Where(c => c.Location.Distance(point) < radius)
            .ToListAsync();
    }
}