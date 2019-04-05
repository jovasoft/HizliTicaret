using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class popupstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Popups");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Popups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Popups");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Popups",
                nullable: false,
                defaultValue: 0);
        }
    }
}
