using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnackVote_Backend.Migrations
{
    public partial class AddedHasVoted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasVoted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasVoted",
                table: "Users");
        }
    }
}
