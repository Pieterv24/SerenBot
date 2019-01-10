using Microsoft.EntityFrameworkCore.Migrations;

namespace SerenBot.Migrations
{
    public partial class AddedUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "NotificationUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "NotificationUsers");
        }
    }
}
