using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class FixEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rounds_LoserId",
                table: "Rounds",
                column: "LoserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_WinnerId",
                table: "Rounds",
                column: "WinnerId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Accounts_LoserId",
                table: "Rounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Accounts_WinnerId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_LoserId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_WinnerId",
                table: "Rounds");
        }
    }
}
