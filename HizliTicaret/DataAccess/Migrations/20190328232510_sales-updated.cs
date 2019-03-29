using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.Migrations
{
    public partial class salesupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
       name: "Sales",
       columns: table => new
       {
           Id = table.Column<Guid>(nullable: false),
           ProductId = table.Column<Guid>(nullable: false),
           MerchantUserName = table.Column<string>(nullable: true),
           UserName = table.Column<string>(nullable: true),
           Date = table.Column<DateTime>(nullable: true)
       },
       constraints: table =>
       {
           table.PrimaryKey("PK_Sales", x => x.Id);
       });

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Sales",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Sales");
        }
    }
}
