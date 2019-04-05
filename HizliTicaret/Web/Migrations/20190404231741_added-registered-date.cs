using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class addedregistereddate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "AspNetUsers");
        }
    }
}
