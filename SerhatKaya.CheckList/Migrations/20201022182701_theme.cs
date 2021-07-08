using Microsoft.EntityFrameworkCore.Migrations;

namespace SerhatKaya.CheckList.Migrations
{
    public partial class theme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "CheckLists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theme",
                table: "CheckLists");
        }
    }
}
