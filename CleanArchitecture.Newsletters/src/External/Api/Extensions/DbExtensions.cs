using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Api.Extensions;

public static class DbExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
