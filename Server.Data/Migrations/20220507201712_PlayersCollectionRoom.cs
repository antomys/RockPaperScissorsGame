using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Dal.Migrations
{
    public partial class PlayersCollectionRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Players_PlayerId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_PlayerId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_RoomId",
                table: "Players",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Rooms_RoomId",
                table: "Players",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Rooms_RoomId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_RoomId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Rooms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PlayerId",
                table: "Rooms",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Players_PlayerId",
                table: "Rooms",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
