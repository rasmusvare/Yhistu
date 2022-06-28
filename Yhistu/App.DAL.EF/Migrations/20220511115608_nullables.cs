using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class nullables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssociationId",
                table: "MeterTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NextCheck",
                table: "Meters",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "InstalledOn",
                table: "Meters",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckedOn",
                table: "Meters",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Meters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssociationId",
                table: "MeasuringUnits",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeterTypes_AssociationId",
                table: "MeterTypes",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasuringUnits_AssociationId",
                table: "MeasuringUnits",
                column: "AssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeasuringUnits_Associations_AssociationId",
                table: "MeasuringUnits",
                column: "AssociationId",
                principalTable: "Associations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeterTypes_Associations_AssociationId",
                table: "MeterTypes",
                column: "AssociationId",
                principalTable: "Associations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeasuringUnits_Associations_AssociationId",
                table: "MeasuringUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_MeterTypes_Associations_AssociationId",
                table: "MeterTypes");

            migrationBuilder.DropIndex(
                name: "IX_MeterTypes_AssociationId",
                table: "MeterTypes");

            migrationBuilder.DropIndex(
                name: "IX_MeasuringUnits_AssociationId",
                table: "MeasuringUnits");

            migrationBuilder.DropColumn(
                name: "AssociationId",
                table: "MeterTypes");

            migrationBuilder.DropColumn(
                name: "AssociationId",
                table: "MeasuringUnits");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NextCheck",
                table: "Meters",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "InstalledOn",
                table: "Meters",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CheckedOn",
                table: "Meters",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Meters",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
