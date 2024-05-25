using LBS.Contracts;
using LBS.Contracts.Dto;
using LBS.Dal.Models;
using LBS.Dal.Repositories;

namespace LBS.Core.Services;

public class NamedLocationService : INamedLocationService
{
    private readonly INamedLocationRepository _namedLocationRepository;

    public NamedLocationService(INamedLocationRepository namedLocationRepository)
    {
        _namedLocationRepository = namedLocationRepository;
    }
    public async Task CreateAsync(CreateNamedLocationInputDto input)
    {
        var namedLocation = new NamedLocation
        {
            Name = input.Name,
            Location = new NetTopologySuite.Geometries.Point(input.Longitude, input.Latitude)
        };
        await _namedLocationRepository.CreateAsync(namedLocation);
        await _namedLocationRepository.SaveChangeAsync();
    }

    public async Task<List<NamedLocationDto>> GetNearbyLocationAsync(GetNearbyLocationsInputDto input)
    {
        var point = new NetTopologySuite.Geometries.Point(input.Longitude, input.Latitude);
        var locations = await _namedLocationRepository.GetNearbyLocationsAsync(point, input.Radius);
        return locations.Select(x => new NamedLocationDto
        {
            Id = x.Id,
            Name = x.Name,
            Latitude = x.Location.Y,
            Longitude = x.Location.X
        }).ToList();
    }
}