using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    IsPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsFull = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreationTimeTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdateTicks = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: true),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Loss = table.Column<int>(type: "INTEGER", nullable: false),
                    Draws = table.Column<int>(type: "INTEGER", nullable: false),
                    WinLossRatio = table.Column<double>(type: "REAL", nullable: false),
                    TimeSpent = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    UsedRock = table.Column<int>(type: "INTEGER", nullable: false),
                    UsedPaper = table.Column<int>(type: "INTEGER", nullable: false),
                    UsedScissors = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statistics_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    RoomId = table.Column<string>(type: "TEXT", nullable: true),
                    WinnerId = table.Column<string>(type: "TEXT", nullable: true),
                    LoserId = table.Column<string>(type: "TEXT", nullable: true),
                    IsFinished = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartTimeTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    FinishTimeTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdateTicks = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Round_Account_LoserId",
                        column: x => x.LoserId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Round_Account_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Round_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: true),
                    IsReady = table.Column<bool>(type: "INTEGER", nullable: false),
                    Move = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<string>(type: "TEXT", nullable: true),
                    RoundId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Player_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Player_Round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Round",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_AccountId",
                table: "Player",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoomId",
                table: "Player",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoundId",
                table: "Player",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_LoserId",
                table: "Round",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_RoomId",
                table: "Round",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Round_WinnerId",
                table: "Round",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AccountId",
                table: "Statistics",
                column: "AccountId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Room");
        }
    }
}
