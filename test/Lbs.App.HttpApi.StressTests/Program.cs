using LBS.Contracts.Dto;
using LBS.Sdk;
using PowerThreadPool;
using PowerThreadPool.Options;

namespace Lbs.App.HttpApi.StressTests;

public static class Program
{
    public static void Main(string[] args)
    {
        var factory = new ApiConnectionFactory();
        var api = factory.CreateLbsApi();
        if (!api.IsHealthy())
        {
            Console.WriteLine("API is not healthy");
            return;
        }
        Console.WriteLine("API is healthy");

        Task.Delay(TimeSpan.FromSeconds(2)).Wait();
        GenerateTestData(api);
        Task.Delay(TimeSpan.FromSeconds(10)).Wait();
        TestGetLocations(api);
        Task.Delay(TimeSpan.FromSeconds(2)).Wait();
    }

    private static void TestGetLocations(ILbsApi api)
    {
        const int count = 5000;
        Console.WriteLine($"Starting get nearest location test with count {count}...");
        var pool = new PowerPool(new PowerPoolOption
        {
            MaxThreads = 10,
            QueueType = QueueType.FIFO,
        });
        var startTime = DateTime.Now;
        for (var i = 0; i < count; i++)
        {
            pool.QueueWorkItem(() => api.NamedLocation.GetNearbyLocationAsync(new GetNearbyLocationsInputDto
            {
                Latitude = RandomLatitude(),
                Longitude = RandomLongitude(),
                Radius = 1000
            }).Result);
        }

        while (pool.WaitingWorkCount != 0)
        {
            Task.Delay(100).Wait();
        }
        var endTime = DateTime.Now;
        Console.WriteLine($"Total failed requests: {pool.FailedWorkCount}");
        Console.WriteLine($"Time taken for get nearest: {endTime - startTime}");
    }

    private static void GenerateTestData(ILbsApi api)
    {
        const int count = 100000;
        var pool = new PowerPool(new PowerPoolOption
        {
            MaxThreads = 10,
            QueueType = QueueType.FIFO,
        });
        Console.WriteLine($"Starting data generation with count {count}...");
        var startTime = DateTime.Now;
        for (var i = 0; i < count; i++)
        {
            pool.QueueWorkItem(() => api.NamedLocation.CreateAsync(new CreateNamedLocationInputDto
            {
                Name = RandomName(),
                Latitude = RandomLatitude(),
                Longitude = RandomLongitude()
            }).Wait());
        }

        while (pool.WaitingWorkCount != 0)
        {
            Task.Delay(100).Wait();
        }
        var endTime = DateTime.Now;
        Console.WriteLine($"Total failed requests: {pool.FailedWorkCount}");
        Console.WriteLine($"Time taken for insert: {endTime - startTime}");
    }

    private static float RandomLongitude()
    {
        return (float)(-180 + 360 * new Random().NextDouble());
    }

    private static float RandomLatitude()
    {
        return (float)(-90 + 180 * new Random().NextDouble());
    }

    private static string RandomName()
    {
        return Guid.NewGuid().ToString();
    }
}