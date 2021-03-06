using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class apartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Apartments");

            migrationBuilder.AddColumn<bool>(
                name: "IsBusiness",
                table: "Apartments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBusiness",
                table: "Apartments");

            migrationBuilder.AddColumn<char>(
                name: "Type",
                table: "Apartments",
                type: "character(1)",
                nullable: false,
                defaultValue: ' ');
        }
    }
}
