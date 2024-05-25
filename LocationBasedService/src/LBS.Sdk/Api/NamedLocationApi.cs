using System.Text;
using System.Text.Json;
using LBS.Contracts;
using LBS.Contracts.Dto;

namespace LBS.Sdk.Api;

public class NamedLocationApi : INamedLocationService
{
    private readonly ILbsApi _lbsApi;

    public NamedLocationApi(ILbsApi lbsApi)
    {
        _lbsApi = lbsApi;
    }
    
    public Task CreateAsync(CreateNamedLocationInputDto input)
    {
        var httpClient = _lbsApi.HttpClient;
        var request = new HttpRequestMessage(HttpMethod.Post, "api/lbs/locations")
        {
            Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json")
        };
        return httpClient.SendAsync(request);
    }

    public async Task<List<NamedLocationDto>> GetNearbyLocationAsync(GetNearbyLocationsInputDto input)
    {
        var httpClient = _lbsApi.HttpClient;
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/lbs/locations/nearest?longitude={input.Longitude}&latitude={input.Latitude}&radius={input.Radius}");

        var locations = await await httpClient.SendAsync(request)
            .ContinueWith(async responseTask =>
            {
                var response = await responseTask;
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NamedLocationDto>>(content);
            });
        return locations!;
    }
}