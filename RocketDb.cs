using Microsoft.EntityFrameworkCore;

class RocketDb : DbContext
{
    public RocketDb(DbContextOptions<RocketDb> options)
        :base(options) {}
    public DbSet<Rocket> Rockets => Set<Rocket>();
}