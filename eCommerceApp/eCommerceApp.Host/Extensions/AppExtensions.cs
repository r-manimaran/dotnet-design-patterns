using eCommerceApp.Infrastructure.Data;

namespace eCommerceApp.Host.Extensions;

public static class AppExtensions
{
    public static async void ApplyMigration(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
        }
    }
}
