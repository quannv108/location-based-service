namespace LBS.Sdk;

public interface IApiConnectionFactory
{
    ILbsApi CreateLbsApi();
}