var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache").WithRedisInsight();

builder.AddProject<Projects.ReadThroughCache>("readthroughcache").WithReference(redis).WaitFor(redis);

builder.Build().Run();
