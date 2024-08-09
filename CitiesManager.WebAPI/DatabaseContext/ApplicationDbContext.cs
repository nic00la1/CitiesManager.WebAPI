using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DatabaseContext;

public class ApplicationDbContext : DbContext
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
