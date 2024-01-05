using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public class Db: DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Category> Categories{ get; set; }
    public DbSet<Developer> Developers{ get; set; }
    public DbSet<User> Users{ get; set; }

    public Db(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameCategory>().HasKey(gc => new { gc.GameId, gc.CategoryId});
    }
}
