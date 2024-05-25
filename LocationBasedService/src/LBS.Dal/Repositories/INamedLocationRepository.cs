using LBS.Dal.Models;
using NetTopologySuite.Geometries;

namespace LBS.Dal.Repositories;

public interface INamedLocationRepository : IRepository<NamedLocation>
{
    Task<List<NamedLocation>> GetNearbyLocationsAsync(Point point, double radius);
}