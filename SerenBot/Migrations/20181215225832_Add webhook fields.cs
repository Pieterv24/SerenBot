using Microsoft.EntityFrameworkCore.Migrations;

namespace SerenBot.Migrations
{
    public partial class Addwebhookfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationChannel",
                table: "Guilds",
                newName: "NotificationWebhookId");

            migrationBuilder.AddColumn<string>(
                name: "NotificationWebhookToken",
                table: "Guilds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationWebhookToken",
                table: "Guilds");

            migrationBuilder.RenameColumn(
                name: "NotificationWebhookId",
                table: "Guilds",
                newName: "NotificationChannel");
        }
    }
}
