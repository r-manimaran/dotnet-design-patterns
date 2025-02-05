using Marten;
using Scalar.AspNetCore;
using ToDoApi.Endpoints;
using ToDoApi.Events;
using ToDoApi.Projections;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("postgresql")!);

    options.UseSystemTextJsonForSerialization();
    
    options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;

    options.Events.AddEventTypes(new[]
    {
        typeof(TodoCreated),
        typeof(TodoUpdated),
        typeof(TodoDeleted),
        typeof(TodoCompleted)
    });
    options.Projections.Add<TodoProjection>(Marten.Events.Projections.ProjectionLifecycle.Inline);


});

builder.Services.AddSingleton<TodoEndpoints>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // /swagger
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "OpenApi v1"));

    // /scalar/v1
    app.MapScalarApiReference();

    // /api-docs/
    app.UseReDoc(options => {
        options.SpecUrl("/openapi/v1.json");
    });
}

TodoEndpoints.MapToDoEndpoints(app);

app.UseHttpsRedirection();

app.Run();

