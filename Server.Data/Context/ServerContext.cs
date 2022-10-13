using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data.Context;

public sealed class ServerContext : DbContext
{
    public DbSet<Account> Accounts { get; init; }
    
    public DbSet<Room> Rooms { get; init; }
    
    
    public DbSet<Player> Players { get; init; }
    
    public DbSet<Round> Rounds { get; init; }
    
    public DbSet<Statistics> StatisticsEnumerable { get; init; }

    public ServerContext(DbContextOptions<ServerContext> contextOptions)
        :base(contextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Round>()
            .HasQueryFilter(round => !round.IsFinished);
    }
}