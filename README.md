## Summary
This project for testing the ability of postgresql with postGIS in handle search for nearby locations.

Project structure is designed for easy to use and extend, using DDD approach.

![Project structure](./docs/images/project_structure.png)

### Test constraints
* Machine specification:
  * CPU: 13th Gen Intel(R) Core(TM) i5-13600K   3.50 GHz
  * RAM: 32GB bus 3200MHz
  * GPU: No
  * SSD: SSD Samsung 980 Pro 1TB
* Testing method: run system on `docker compose`
* StressTest project run in Jetbrain Rider
* Thread pool limit 10
* Request for nearby location: 5000, radius = 1000

### How to run
1. Run `docker-compose build` to build the system
2. Run `docker-compose up` to start the system
3. Run `StressTests` project to test the system

### Test1: 100K locations with naive implementation
We have naive implementation that store directly the coordinates of locations in the database.
```csharp
[Table("NamedLocations", Schema = "lbs")]
public class NamedLocation
{
    [Key] public Guid Id { get; set; }

    [NotNull] [MaxLength(128)]
    public string Name { get; set; }
    
    [Column(TypeName = "geometry (point)")]
    public Point Location { get; set; }
}
```

Result of simple implementation (no index):
```text
API is healthy
Starting data generation with count 100000...
Total failed requests: 0
Time taken for insert: 00:00:42.6682731
Starting get nearest location test with count 5000...
Total failed requests: 0
Time taken for get nearest: 00:12:27.5246261
```

Result of simple implementation (with index in `location` column):
```text
API is healthy
Starting data generation with count 100000...
Total failed requests: 0
Time taken for insert: 00:00:43.9397544
Starting get nearest location test with count 5000...
Total failed requests: 0
Time taken for get nearest: 00:11:59.9639587

```

=> Add Index doesn't have effect on the column

Summary:
* PostGIS can handle pretty well for small set of locations. Without need of advance algorithm.
* I was monitor the log of database layer and able to calculate that average response time `100ms` for 5000 fetch requests. But this calculation and monitoring may not accurate, because it's just a simple calculation based on the log.
* Calculation from stress test output, 747 sec for 5000 http requests, so 1 request take 149ms sec. We will use this number to compare the performance of the system in the next test because it's more easy.


### Test2: 10M locations with naive implementation
This test consumed too much time to insert data into database so I have to stop it.


### Test3: 100K locations with Geohash implementation
* Read about Geohash: https://en.wikipedia.org/wiki/Geohash
* We have a new implementation that store the geohash of location in the database.
* We will skip errors from Geohash implement mention [here](https://en.wikipedia.org/wiki/Geohash#Limitations_when_used_for_deciding_proximity) to focus only on performance difference.
* We will use Geohash precision = 6, which is the most accurate level for radius 1000m. See [here](https://en.wikipedia.org/wiki/Geohash#Digits_and_precision_in_km).
* We also implement to fix boundary problem of Geohash, re-use the package [geohash-dotnet](https://www.nuget.org/packages/geohash-dotnet).

In Test3, we will try to retrieve all locations in the same geohash cell with the target location, then calculate the distance between them to find the nearest location in **application** memory

```text
API is healthy
Starting data generation with count 100000...
Total failed requests: 0
Time taken for insert: 00:00:42.3658457
Starting get nearest location test with count 5000...
Total failed requests: 0
Time taken for get nearest: 00:00:01.3071433
```
Total time spends for 5000 requests is 1.3 sec, so 1 request take 0.26 ms.
Awesome, the performance is improved significantly. The time to get nearest location is reduced from 149ms to <1ms. It's a huge improvement.

### Test4: 100K locations with Geohash implementation
In Test4, we will try to retrieve all locations in the same geohash cell with the target location, then calculate the distance between them to find the nearest location in **database** layer

```text
API is healthy
Starting data generation with count 100000...
Total failed requests: 0
Time taken for insert: 00:00:42.8159021
Starting get nearest location test with count 5000...
Total failed requests: 0
Time taken for get nearest: 00:00:01.4206748
```

Look like the performance is not improved much, it's even slower than the previous test.
The reason is that we have to calculate the distance between locations in the database layer, which is look like not optimized for this kind of operation.

## Conclusion
* PostGIS can handle pretty well for small set of locations. Without need of advance algorithm.
* Geohash is a good solution for searching nearby locations. It's easy to implement and have good performance.
* The performance of Geohash implementation is better than naive implementation. But the performance is not improved much when we calculate the distance between locations in the database layer.