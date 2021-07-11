using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomPlayersId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFinished = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeFinished = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: true),
                    Loss = table.Column<int>(type: "INTEGER", nullable: true),
                    Draws = table.Column<int>(type: "INTEGER", nullable: true),
                    WinLossRatio = table.Column<double>(type: "REAL", nullable: true),
                    TimeSpent = table.Column<string>(type: "TEXT", nullable: true),
                    UsedRock = table.Column<int>(type: "INTEGER", nullable: true),
                    UsedPaper = table.Column<int>(type: "INTEGER", nullable: true),
                    UsedScissors = table.Column<int>(type: "INTEGER", nullable: true),
                    Score = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    StatisticsId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoomPlayerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Statistics_StatisticsId",
                        column: x => x.StatisticsId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoundId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsReady = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsFull = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRoundEnded = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstPlayerMove = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondPlayerMove = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomPlayers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomPlayers_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoomPlayerId",
                table: "Accounts",
                column: "RoomPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_StatisticsId",
                table: "Accounts",
                column: "StatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoomId",
                table: "RoomPlayers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoundId",
                table: "RoomPlayers",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomPlayerId",
                table: "Rooms",
                column: "RoomPlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoundId",
                table: "Rooms",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_LoserId",
                table: "Rounds",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RoomPlayersId",
                table: "Rounds",
                column: "RoomPlayersId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_WinnerId",
                table: "Rounds",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AccountId",
                table: "Statistics",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Accounts_LoserId",
                table: "Rounds",
                column: "LoserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Accounts_WinnerId",
                table: "Rounds",
                column: "WinnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                table: "Rounds",
                column: "RoomPlayersId",
                principalTable: "RoomPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Accounts_AccountId",
                table: "Statistics",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_RoomPlayers_RoomPlayerId",
                table: "Accounts",
                column: "RoomPlayerId",
                principalTable: "RoomPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomPlayers_RoomPlayerId",
                table: "Rooms",
                column: "RoomPlayerId",
                principalTable: "RoomPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_RoomPlayers_RoomPlayerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomPlayers_RoomPlayerId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Statistics_StatisticsId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "RoomPlayers");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
