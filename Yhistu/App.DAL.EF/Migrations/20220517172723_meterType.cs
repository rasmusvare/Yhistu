using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class meterType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MeterTypes");

            migrationBuilder.AlterColumn<Guid>(
                name: "RelationshipTypeId",
                table: "ApartmentPersons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "ApartmentPersons",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "ApartmentPersons");

            migrationBuilder.AddColumn<char>(
                name: "Type",
                table: "MeterTypes",
                type: "character(1)",
                nullable: false,
                defaultValue: ' ');

            migrationBuilder.AlterColumn<Guid>(
                name: "RelationshipTypeId",
                table: "ApartmentPersons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
