using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Statistics> StatisticsEnumerable { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<ActiveSessions> ActiveSessionsEnumerable { get; set; }
    }
}