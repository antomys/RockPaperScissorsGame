using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class NewFluentAssertion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_RoomPlayersId",
                table: "Rounds");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RoomPlayersId",
                table: "Rounds",
                column: "RoomPlayersId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                table: "Rounds",
                column: "RoomPlayersId",
                principalTable: "RoomPlayers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_RoomPlayersId",
                table: "Rounds");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_RoomPlayersId",
                table: "Rounds",
                column: "RoomPlayersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_RoomPlayers_RoomPlayersId",
                table: "Rounds",
                column: "RoomPlayersId",
                principalTable: "RoomPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
