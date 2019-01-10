using Microsoft.EntityFrameworkCore.Migrations;

namespace SerenBot.Migrations
{
    public partial class ChangedNotificationChannelType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "NotificationChannel",
                table: "Guilds",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NotificationChannel",
                table: "Guilds",
                nullable: true,
                oldClrType: typeof(ulong));
        }
    }
}
