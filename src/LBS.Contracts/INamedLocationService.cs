using LBS.Contracts.Dto;

namespace LBS.Contracts;

public interface INamedLocationService
{
    Task CreateAsync(CreateNamedLocationInputDto input);
    Task<List<NamedLocationDto>> GetNearbyLocationAsync(GetNearbyLocationsInputDto input);
}