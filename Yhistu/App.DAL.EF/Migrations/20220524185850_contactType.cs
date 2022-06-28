using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class contactType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssociationId",
                table: "ContactTypes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactTypes_AssociationId",
                table: "ContactTypes",
                column: "AssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactTypes_Associations_AssociationId",
                table: "ContactTypes",
                column: "AssociationId",
                principalTable: "Associations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactTypes_Associations_AssociationId",
                table: "ContactTypes");

            migrationBuilder.DropIndex(
                name: "IX_ContactTypes_AssociationId",
                table: "ContactTypes");

            migrationBuilder.DropColumn(
                name: "AssociationId",
                table: "ContactTypes");
        }
    }
}
