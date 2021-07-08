using Microsoft.EntityFrameworkCore.Migrations;

namespace SerhatKaya.CheckList.Migrations
{
    public partial class catTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Categories");
        }
    }
}
