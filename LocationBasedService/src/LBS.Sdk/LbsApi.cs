using LBS.Contracts;
using LBS.Sdk.Api;

namespace LBS.Sdk;

public class LbsApi : ILbsApi
{
    public HttpClient HttpClient { get; }

    public INamedLocationService NamedLocation { get; }
    
    public LbsApi()
    {
        HttpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5004"),
            DefaultRequestHeaders =
            {
                { "Accept", "application/json" }
            }
        };
        NamedLocation = new NamedLocationApi(this);
    }
    
    public bool IsHealthy()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "health");
        var response = HttpClient.SendAsync(request).Result;
        return response.IsSuccessStatusCode;
    }
}