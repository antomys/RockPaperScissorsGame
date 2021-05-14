using Microsoft.EntityFrameworkCore;
using Server.Dal.Entities;

namespace Server.Dal.Context
{
    public class ServerContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPlayers> RoomPlayersEnumerable { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Statistics> StatisticsEnumerable { get; set; }

        public ServerContext(DbContextOptions<ServerContext> contextOptions)
            :base(contextOptions) { }
    }
}