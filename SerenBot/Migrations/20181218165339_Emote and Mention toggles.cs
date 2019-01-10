using Microsoft.EntityFrameworkCore.Migrations;

namespace SerenBot.Migrations
{
    public partial class EmoteandMentiontoggles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmotesEnabled",
                table: "Guilds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MentionsEnabled",
                table: "Guilds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmotesEnabled",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "MentionsEnabled",
                table: "Guilds");
        }
    }
}
