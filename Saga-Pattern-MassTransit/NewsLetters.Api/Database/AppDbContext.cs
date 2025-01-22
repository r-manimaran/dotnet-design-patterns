using Microsoft.EntityFrameworkCore;
using NewsLetters.Api.Sagas;

namespace NewsLetters.Api.Database;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<NewsletterOnboardingSagaData> SagaData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsletterOnboardingSagaData>().HasKey(x => x.CorrelationId);
    }

}
