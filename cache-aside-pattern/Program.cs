using Dapper;
using Bogus;
using Npgsql;
using Microsoft.Extensions.Caching.Distributed;
using cache_aside_pattern;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
var dataSource = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection")).Build();
builder.Services.AddSingleton(dataSource);

builder.Services.AddStackExchangeRedisCache(setup => {
    setup.Configuration =  "127.0.0.1:6379"; //builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users/{id}", async([FromRoute] int id, 
                                [FromServices] NpgsqlDataSource source,
                                [FromServices] IDistributedCache cache) =>
{
    string key = $"user-{id}";
   
    User? user = await cache.GetOrCreateAsync(key, async token=> {
        var user = await GetUserAsync();
        return user; 
    });


    if (user is null) {
        return Results.NotFound();
    }

   return Results.Ok(user);

      // local function
   async Task<User> GetUserAsync(){
    await using var conn = await source.OpenConnectionAsync();

    var userFromDb = await conn.QuerySingleAsync<User>(
    @"SELECT 
            id as Id,
            firstname as FirstName,
            lastname as LastName,
            dateofbirth as DateOfBirth
        FROM users WHERE id = @userId
    ", new { userId=id });
    return userFromDb;
   }
}).WithName("GetUser");

app.MapPost("/users", async([FromServices] NpgsqlDataSource source) =>
{
  var faker = new Faker();

  await using var conn = await source.OpenConnectionAsync();
    
  var userId =  await conn.ExecuteAsync(
        @"INSERT INTO users (firstname, lastname, dateofbirth)
        VALUES (@FirstName, @LastName, @DateOfBirth)
        RETURNING id;",
        new User {
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            DateOfBirth = faker.Date.Past(50, DateTime.Now).Date
        });
    return Results.CreatedAtRoute("GetUser", new { id = userId },userId);
}).WithName("CreateUser");

app.Run();

