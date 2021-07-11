﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Dal.Context;

namespace Server.Dal.Migrations
{
    [DbContext(typeof(ServerContext))]
    partial class ServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Server.Dal.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RoomPlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("StatisticsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomPlayerId");

                    b.HasIndex("StatisticsId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Server.Dal.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFull")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsReady")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRoundEnded")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomPlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoundId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomPlayerId")
                        .IsUnique();

                    b.HasIndex("RoundId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Server.Dal.Entities.RoomPlayers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FirstPlayerMove")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoundId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SecondPlayerMove")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("RoundId");

                    b.ToTable("RoomPlayers");
                });

            modelBuilder.Entity("Server.Dal.Entities.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomPlayersId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("TimeFinished")
                        .HasColumnType("TEXT");

                    b.Property<int>("WinnerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LoserId");

                    b.HasIndex("RoomPlayersId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Server.Dal.Entities.Statistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Draws")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Loss")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Score")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TimeSpent")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UsedPaper")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UsedRock")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UsedScissors")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("WinLossRatio")
                        .HasColumnType("REAL");

                    b.Property<int?>("Wins")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("Server.Dal.Entities.Account", b =>
                {
                    b.HasOne("Server.Dal.Entities.RoomPlayers", "RoomPlayers")
                        .WithMany("Accounts")
                        .HasForeignKey("RoomPlayerId");

                    b.HasOne("Server.Dal.Entities.Statistics", "Statistics")
                        .WithMany()
                        .HasForeignKey("StatisticsId");

                    b.Navigation("RoomPlayers");

                    b.Navigation("Statistics");
                });

            modelBuilder.Entity("Server.Dal.Entities.Room", b =>
                {
                    b.HasOne("Server.Dal.Entities.RoomPlayers", "RoomPlayers")
                        .WithOne()
                        .HasForeignKey("Server.Dal.Entities.Room", "RoomPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Dal.Entities.Round", "Round")
                        .WithMany()
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoomPlayers");

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Server.Dal.Entities.RoomPlayers", b =>
                {
                    b.HasOne("Server.Dal.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Dal.Entities.Round", "Round")
                        .WithMany()
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Server.Dal.Entities.Round", b =>
                {
                    b.HasOne("Server.Dal.Entities.Account", "Loser")
                        .WithMany()
                        .HasForeignKey("LoserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Dal.Entities.RoomPlayers", "RoomPlayers")
                        .WithMany()
                        .HasForeignKey("RoomPlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Dal.Entities.Account", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Loser");

                    b.Navigation("RoomPlayers");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("Server.Dal.Entities.Statistics", b =>
                {
                    b.HasOne("Server.Dal.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Server.Dal.Entities.RoomPlayers", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
