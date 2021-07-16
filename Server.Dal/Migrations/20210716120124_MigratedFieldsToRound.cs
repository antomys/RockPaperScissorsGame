using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class MigratedFieldsToRound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPlayerMove",
                table: "RoomPlayers");

            migrationBuilder.DropColumn(
                name: "SecondPlayerMove",
                table: "RoomPlayers");

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerMove",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "LastMoveTicks",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerMove",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPlayerMove",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "LastMoveTicks",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "SecondPlayerMove",
                table: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerMove",
                table: "RoomPlayers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerMove",
                table: "RoomPlayers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
