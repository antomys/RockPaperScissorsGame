using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Dal.Migrations
{
    public partial class PlayerReadyFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Players");
        }
    }
}
