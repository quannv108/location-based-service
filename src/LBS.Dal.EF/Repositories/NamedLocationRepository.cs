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
    
    public Task<List<NamedLocation>> GetNearbyLocationsAsync(Point point, double radiusInMeters)
    {
        return Queryable()
            .Where(c => c.Location.Distance(point) < radiusInMeters)
            .ToListAsync();
    }

    public Task<List<NamedLocation>> GetNearbyLocationsByGeoHashAsync(IReadOnlyList<string> geoHashes)
    {
        return Queryable()
            .Where(x => geoHashes.Contains(x.GeoHash))
            .ToListAsync();
    }

    public Task<List<NamedLocation>> GetNearbyLocationsByGeoHashAsync(IReadOnlyList<string> geoHashes, double radiusInMeters)
    {
        return Queryable()
            .Where(x => geoHashes.Contains(x.GeoHash) 
                        && x.Location.Distance(x.Location) < radiusInMeters)
            .ToListAsync();
    }
}