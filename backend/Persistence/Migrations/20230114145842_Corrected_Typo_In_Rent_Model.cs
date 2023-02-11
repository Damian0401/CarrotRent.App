using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Corrected_Typo_In_Rent_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Users_RecieverId",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "Rents",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_RecieverId",
                table: "Rents",
                newName: "IX_Rents_ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Users_ReceiverId",
                table: "Rents",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Users_ReceiverId",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Rents",
                newName: "RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_ReceiverId",
                table: "Rents",
                newName: "IX_Rents_RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Users_RecieverId",
                table: "Rents",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
