using Microsoft.EntityFrameworkCore.Migrations;

namespace SerhatKaya.CheckList.Migrations
{
    public partial class logs_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_CheckListId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UserId",
                table: "Logs");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CheckListId",
                table: "Logs",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_CheckListId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UserId",
                table: "Logs");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CheckListId",
                table: "Logs",
                column: "CheckListId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId",
                unique: true);
        }
    }
}
