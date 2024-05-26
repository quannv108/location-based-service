using LBS.Contracts;

namespace LBS.Sdk;

public interface ILbsApi
{
    internal HttpClient HttpClient { get; }
    bool IsHealthy();
    INamedLocationService NamedLocation { get; }
}