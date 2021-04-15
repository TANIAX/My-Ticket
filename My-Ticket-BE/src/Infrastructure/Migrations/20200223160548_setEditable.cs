using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Migrations
{
    public partial class setEditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Type",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Status",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Satisfaction",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Editable",
                table: "Priority",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Type");

            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Satisfaction");

            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Editable",
                table: "Priority");
        }
    }
}
