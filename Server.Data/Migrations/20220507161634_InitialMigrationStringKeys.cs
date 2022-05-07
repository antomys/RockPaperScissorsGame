using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Dal.Migrations
{
    public partial class InitialMigrationStringKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
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
                        name: "FK_Statistics_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: true),
                    Move = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    PlayerId = table.Column<string>(type: "TEXT", nullable: true),
                    IsPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsFull = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreationTimeTicks = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    RoomId = table.Column<string>(type: "TEXT", nullable: true),
                    WinnerId = table.Column<string>(type: "TEXT", nullable: true),
                    LoserId = table.Column<string>(type: "TEXT", nullable: true),
                    IsFinished = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_Accounts_LoserId",
                        column: x => x.LoserId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rounds_Accounts_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rounds_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_AccountId",
                table: "Players",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_RoundId",
                table: "Players",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PlayerId",
                table: "Rooms",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_LoserId",
                table: "Rounds",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RoomId",
                table: "Rounds",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_WinnerId",
                table: "Rounds",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AccountId",
                table: "Statistics",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Rounds_RoundId",
                table: "Players",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Accounts_AccountId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Accounts_LoserId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Accounts_WinnerId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Rounds_RoundId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
