using EventSourcing.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgresql")
                      .WithDataVolume()
                      .WithPgAdmin();

database.AddDatabase("eventsourcing");

var apiService = builder.AddProject<Projects.ToDoApi>("todoapi")
                       .WithReference(database)
                       .WaitFor(database);

apiService.WithSwaggerUi()
          .WithScalarUi()
          .WithRedocUi();

builder.Build().Run();
