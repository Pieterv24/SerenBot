using Microsoft.EntityFrameworkCore.Migrations;

namespace SerenBot.Migrations
{
    public partial class AddedEmojiIdstoguilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "AmloddEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "CadarnEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "CrwysEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "HefinEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "IorwerthEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "IthellEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "MeilyrEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "TrahaearnEmojiId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmloddEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "CadarnEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "CrwysEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "HefinEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "IorwerthEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "IthellEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "MeilyrEmojiId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "TrahaearnEmojiId",
                table: "Guilds");
        }
    }
}
