using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Changed_UserData_Relation_From_OneToOne_To_OneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserDataId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserDataId",
                table: "Users",
                column: "UserDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserDataId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserDataId",
                table: "Users",
                column: "UserDataId",
                unique: true);
        }
    }
}
