using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Dal.Migrations
{
    public partial class ChangedDateTimeOffsetToTicks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFinished",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Rooms");

            migrationBuilder.AddColumn<long>(
                name: "TimeFinishedTicks",
                table: "Rounds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreationTimeTicks",
                table: "Rooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFinishedTicks",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "CreationTimeTicks",
                table: "Rooms");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeFinished",
                table: "Rounds",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationTime",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
