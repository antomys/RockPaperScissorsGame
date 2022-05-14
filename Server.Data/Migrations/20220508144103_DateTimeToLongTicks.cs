using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Dal.Migrations
{
    public partial class DateTimeToLongTicks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Rounds");

            migrationBuilder.AddColumn<long>(
                name: "FinishTimeTicks",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StartTimeTicks",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishTimeTicks",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "StartTimeTicks",
                table: "Rounds");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FinishTime",
                table: "Rounds",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartTime",
                table: "Rounds",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
