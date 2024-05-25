namespace LBS.Sdk;

public class ApiConnectionFactory : IApiConnectionFactory
{
    public ILbsApi CreateLbsApi()
    {
        return new LbsApi();
    }
}