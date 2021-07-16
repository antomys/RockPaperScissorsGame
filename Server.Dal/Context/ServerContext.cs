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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomPlayers>()
                .HasOne(invite => invite.FirstPlayer)
                .WithMany(user => user.FirstPlayer)
                .HasForeignKey(invite => invite.FirstPlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomPlayers>()
                .HasOne(players => players.Room)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Room>()
                .HasOne(x => x.RoomPlayers)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomPlayers>()
                .HasOne(invite => invite.SecondPlayer)
                .WithMany(user => user.SecondPlayer)
                .HasForeignKey(invite => invite.SecondPlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Round>()
                .HasOne(x => x.RoomPlayers)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}