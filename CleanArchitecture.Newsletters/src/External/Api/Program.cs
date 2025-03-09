
using Microsoft.EntityFrameworkCore;
using Api.Endpoints;
using Api.Extensions;
using Application.Articles.CreateArticle;
using Domain.Repositories;
using Persistence.Database;
using Persistence.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateArticleCommand).Assembly,
    typeof(AppDbContext).Assembly
));

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddDbContext<AppDbContext>(opt=> {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.ApplyMigrations();
}

app.MapArticleEndpoints();

app.UseHttpsRedirection();

app.Run();

namespace Api
{
    public partial class Program;
    
}
