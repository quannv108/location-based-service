using LBS.Dal.Models;
using NetTopologySuite.Geometries;

namespace LBS.Dal.Repositories;

public interface INamedLocationRepository : IRepository<NamedLocation>
{
    Task<List<NamedLocation>> GetNearbyLocationsAsync(Point point, double radiusInMeters);
    Task<List<NamedLocation>> GetNearbyLocationsByGeoHashAsync(IReadOnlyList<string> geoHashes);
    
    Task<List<NamedLocation>> GetNearbyLocationsByGeoHashAsync(IReadOnlyList<string> geoHashes, double radiusInMeters);
}