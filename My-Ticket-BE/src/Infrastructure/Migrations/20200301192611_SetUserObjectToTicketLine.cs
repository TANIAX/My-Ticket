using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Migrations
{
    public partial class SetUserObjectToTicketLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseBy",
                table: "TicketLine");

            migrationBuilder.AddColumn<string>(
                name: "ResponseById",
                table: "TicketLine",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketLine_ResponseById",
                table: "TicketLine",
                column: "ResponseById");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketLine_User_ResponseById",
                table: "TicketLine",
                column: "ResponseById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketLine_User_ResponseById",
                table: "TicketLine");

            migrationBuilder.DropIndex(
                name: "IX_TicketLine_ResponseById",
                table: "TicketLine");

            migrationBuilder.DropColumn(
                name: "ResponseById",
                table: "TicketLine");

            migrationBuilder.AddColumn<string>(
                name: "ResponseBy",
                table: "TicketLine",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
