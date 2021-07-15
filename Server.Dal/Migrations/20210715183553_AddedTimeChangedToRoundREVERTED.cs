using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class AddedTimeChangedToRoundREVERTED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreatedTicks",
                table: "Rounds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeCreatedTicks",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
