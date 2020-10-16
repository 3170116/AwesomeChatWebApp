using Microsoft.EntityFrameworkCore.Migrations;

namespace AwesomeChat.Migrations
{
    public partial class EditUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ChatGroups_ChatGroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatGroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatGroupId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatGroupId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatGroupId",
                table: "Users",
                column: "ChatGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ChatGroups_ChatGroupId",
                table: "Users",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
