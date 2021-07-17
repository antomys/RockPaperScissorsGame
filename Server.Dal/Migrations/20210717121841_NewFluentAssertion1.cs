using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class NewFluentAssertion1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Statistics_StatisticsId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomPlayers_Rounds_RoundId",
                table: "RoomPlayers");

            migrationBuilder.DropIndex(
                name: "IX_RoomPlayers_RoundId",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsRoundEnded",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoundId",
                table: "RoomPlayers");

            migrationBuilder.AddColumn<int>(
                name: "PlayersCount",
                table: "RoomPlayers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StatisticsId",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Statistics_StatisticsId",
                table: "Accounts",
                column: "StatisticsId",
                principalTable: "Statistics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Statistics_StatisticsId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PlayersCount",
                table: "RoomPlayers");

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRoundEnded",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RoundId",
                table: "RoomPlayers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatisticsId",
                table: "Accounts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPlayers_RoundId",
                table: "RoomPlayers",
                column: "RoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Statistics_StatisticsId",
                table: "Accounts",
                column: "StatisticsId",
                principalTable: "Statistics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomPlayers_Rounds_RoundId",
                table: "RoomPlayers",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
