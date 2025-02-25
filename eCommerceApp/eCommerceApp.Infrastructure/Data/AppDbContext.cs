using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Cart;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions options) :base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get;set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<Archive> CheckoutArchives { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base implementatin to setup Identity tables
        base.OnModelCreating(modelBuilder);

        // custom Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Add the seed data to Add the User Roles
        modelBuilder.Entity<IdentityRole>()
            .HasData(
                        new IdentityRole
                        {
                            Id = "de1d0916-a989-474a-a876-236525ba9f47",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                         new IdentityRole
                         {
                             Id ="d412f56e-9f7f-47d8-a540-2c407940ec44",
                             Name = "User",
                             NormalizedName = "USER"
                         }
                    );

        modelBuilder.Entity<PaymentMethod>().HasData(
            new PaymentMethod
            {
                Id = Guid.Parse("d412f57e-9f7f-47d8-a640-2c407940ec44"),
                Name = "Credit Card"
            });

    }   
}

