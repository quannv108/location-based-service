using Geohash;
using LBS.Contracts;
using LBS.Contracts.Dto;
using LBS.Dal.Models;
using LBS.Dal.Repositories;

namespace LBS.Core.Services;

public class NamedLocationService : INamedLocationService
{
    private readonly INamedLocationRepository _namedLocationRepository;
    private static Geohasher _geohasher = new Geohasher();

    public NamedLocationService(INamedLocationRepository namedLocationRepository)
    {
        _namedLocationRepository = namedLocationRepository;
    }
    public async Task CreateAsync(CreateNamedLocationInputDto input)
    {
        var namedLocation = new NamedLocation
        {
            Name = input.Name,
            Location = new NetTopologySuite.Geometries.Point(input.Longitude, input.Latitude),
            GeoHash = _geohasher.Encode(input.Latitude, input.Longitude, 6) // comment this for test 1, test 2
        };
        await _namedLocationRepository.CreateAsync(namedLocation);
        await _namedLocationRepository.SaveChangeAsync();
    }

    public async Task<List<NamedLocationDto>> GetNearbyLocationAsync(GetNearbyLocationsInputDto input)
    {
        var point = new NetTopologySuite.Geometries.Point(input.Longitude, input.Latitude);
        
        // Test1, Test2: naive implementation
        // var locations = await _namedLocationRepository.GetNearbyLocationsAsync(point, input.Radius);
        
        // Test3:
        var geoHash = _geohasher.Encode(input.Latitude, input.Longitude, 6);
        var nearbyGeoHashes = _geohasher.GetNeighbors(geoHash).Values.ToList();
        var allGeoHashes = new List<string> {geoHash};
        allGeoHashes.AddRange(nearbyGeoHashes);
        var locations = await _namedLocationRepository.GetNearbyLocationsByGeoHashAsync(allGeoHashes);
        locations = locations.Where(x => x.Location.Distance(point) < input.Radius).ToList();
        
        // Test 4
        // var geoHash = _geohasher.Encode(input.Latitude, input.Longitude, 6);
        // var nearbyGeoHashes = _geohasher.GetNeighbors(geoHash).Values.ToList();
        // var allGeoHashes = new List<string> {geoHash};
        // allGeoHashes.AddRange(nearbyGeoHashes);
        // var locations = await _namedLocationRepository.GetNearbyLocationsByGeoHashAsync(allGeoHashes, input.Radius);
        
        return locations.Select(x => new NamedLocationDto
        {
            Id = x.Id,
            Name = x.Name,
            Latitude = x.Location.Y,
            Longitude = x.Location.X
        }).ToList();
    }
}