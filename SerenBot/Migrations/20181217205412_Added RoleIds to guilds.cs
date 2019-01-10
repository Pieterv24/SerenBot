using Microsoft.EntityFrameworkCore.Migrations;

namespace SerenBot.Migrations
{
    public partial class AddedRoleIdstoguilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "AmloddRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "CadarnRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "CrwysRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "HefinRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "IorwerthRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "IthellRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "MeilyrRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "TrahaearnRoleId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmloddRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "CadarnRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "CrwysRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "HefinRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "IorwerthRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "IthellRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "MeilyrRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "TrahaearnRoleId",
                table: "Guilds");
        }
    }
}
