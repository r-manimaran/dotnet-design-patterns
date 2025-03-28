﻿using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace eCommerceApp.Host.Extensions;

public static class AppExtensions
{

    public static void AddApplicationServices(this IServiceCollection services)
    {
        
    }
    public static async void ApplyMigration(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                //await dbContext.Database.EnsureCreatedAsync(); // using this only create db for firsttime. Subsquent migrations will not apply
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
