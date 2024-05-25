## Summary
This project for testing the ability of postgresql with postGIS

Project structure is designed for easy to use and extend, using DDD approach.

### Test constraints
* Generate input item: 100.000 location
* Request for nearby location: 5000, radius = 1000
* Thread pool limit 10
* Machine specification:
  * CPU: 13th Gen Intel(R) Core(TM) i5-13600K   3.50 GHz
  * RAM: 32GB bus 3200MHz
  * GPU: No
  * SSD: SSD Samsung 980 Pro 1TB
* Testing method: run system on `docker compose`
* StressTest project run in Jetbrain Rider

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