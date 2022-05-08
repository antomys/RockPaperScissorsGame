using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data.Context;

public sealed class ServerContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Room> Rooms { get; set; }
    
    
    public DbSet<Player> Players { get; set; }
    
    public DbSet<Round> Rounds { get; set; }
    
    public DbSet<Statistics> StatisticsEnumerable { get; set; }

    public ServerContext(DbContextOptions<ServerContext> contextOptions)
        :base(contextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Round>()
            .HasQueryFilter(x => !x.IsFinished);
    }
}