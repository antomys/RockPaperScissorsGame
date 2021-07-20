using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class NewFluentAssertion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayersCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstPlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondPlayerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPlayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomPlayersId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstPlayerMove = table.Column<int>(type: "INTEGER", nullable: false),
                    SecondPlayerMove = table.Column<int>(type: "INTEGER", nullable: false),
                    WinnerId = table.Column<int>(type: "INTEGER", nullable: true),
                    LoserId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastMoveTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    TimeFinishedTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    IsFinished = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                        column: x => x.RoomPlayersId,
                        principalTable: "RoomPlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomCode = table.Column<string>(type: "TEXT", nullable: true),
                    RoundId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoomPlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsFull = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreationTimeTicks = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomPlayers_RoomPlayerId",
                        column: x => x.RoomPlayerId,
                        principalTable: "RoomPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Loss = table.Column<int>(type: "INTEGER", nullable: false),
                    Draws = table.Column<int>(type: "INTEGER", nullable: false),
                    WinLossRatio = table.Column<double>(type: "REAL", nullable: false),
                    TimeSpent = table.Column<string>(type: "TEXT", nullable: true),
                    UsedRock = table.Column<int>(type: "INTEGER", nullable: false),
                    UsedPaper = table.Column<int>(type: "INTEGER", nullable: false),
                    UsedScissors = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false)
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
                    StatisticsId = table.Column<int>(type: "INTEGER", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_StatisticsId",
                table: "Accounts",
                column: "StatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_FirstPlayerId",
                table: "RoomPlayers",
                column: "FirstPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoomId",
                table: "RoomPlayers",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_SecondPlayerId",
                table: "RoomPlayers",
                column: "SecondPlayerId");

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
                column: "RoomPlayersId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_WinnerId",
                table: "Rounds",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AccountId",
                table: "Statistics",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_Accounts_FirstPlayerId",
                table: "RoomPlayers",
                column: "FirstPlayerId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_Accounts_SecondPlayerId",
                table: "RoomPlayers",
                column: "SecondPlayerId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_Rooms_RoomId",
                table: "RoomPlayers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Accounts_LoserId",
                table: "Rounds",
                column: "LoserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Accounts_WinnerId",
                table: "Rounds",
                column: "WinnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_Accounts_AccountId",
                table: "Statistics",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Statistics_StatisticsId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_Accounts_FirstPlayerId",
                table: "RoomPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_Accounts_SecondPlayerId",
                table: "RoomPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Accounts_LoserId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Accounts_WinnerId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_Rooms_RoomId",
                table: "RoomPlayers");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "RoomPlayers");
        }
    }
}
