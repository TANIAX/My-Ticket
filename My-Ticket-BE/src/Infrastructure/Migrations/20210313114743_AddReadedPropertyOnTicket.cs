using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Migrations
{
    public partial class AddReadedPropertyOnTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Satisfaction");

            migrationBuilder.AddColumn<int>(
                name: "Readed",
                table: "TickerHeader",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Readed",
                table: "TickerHeader");

            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Satisfaction",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
