using CitiesManager.Core.Entities;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext;

public class
    ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
    Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>().HasData(new City()
        {
            Id = Guid.NewGuid(),
            Name = "New York"
        });

        modelBuilder.Entity<City>().HasData(new City()
        {
            Id = Guid.NewGuid(),
            Name = "London"
        });
    }
}
