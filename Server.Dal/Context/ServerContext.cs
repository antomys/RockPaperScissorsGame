﻿using Microsoft.EntityFrameworkCore;
using Server.Dal.Entities;

namespace Server.Dal.Context;

public sealed class ServerContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Player> RoomPlayersEnumerable { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Statistics> StatisticsEnumerable { get; set; }

    public ServerContext(DbContextOptions<ServerContext> contextOptions)
        :base(contextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Round>()
            .HasQueryFilter(x => !x.IsFinished);
            
        // modelBuilder.Entity<RoomPlayer>()
        //     .HasOne(invite => invite.FirstPlayer)
        //     .WithMany(user => user.FirstPlayer)
        //     .HasForeignKey(invite => invite.FirstPlayerId)
        //     .OnDelete(DeleteBehavior.NoAction);
        //
        // modelBuilder.Entity<RoomPlayer>()
        //     .HasOne(players => players.Room)
        //     .WithOne()
        //     .OnDelete(DeleteBehavior.Cascade);
        //     
        // modelBuilder.Entity<Room>()
        //     .HasOne(x => x.RoomPlayer)
        //     .WithOne()
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // modelBuilder.Entity<RoomPlayer>()
        //     .HasOne(invite => invite.SecondPlayer)
        //     .WithMany(user => user.SecondPlayer)
        //     .HasForeignKey(invite => invite.SecondPlayerId)
        //     .OnDelete(DeleteBehavior.NoAction);
        //
        // modelBuilder.Entity<Round>()
        //     .HasOne(x => x.RoomPlayer)
        //     .WithOne()
        //     .OnDelete(DeleteBehavior.NoAction);
    }
}