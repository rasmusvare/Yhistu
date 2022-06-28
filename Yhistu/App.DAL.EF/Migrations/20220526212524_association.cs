using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.DAL.EF.Migrations
{
    public partial class association : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssociationId",
                table: "RelationshipTypes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipTypes_AssociationId",
                table: "RelationshipTypes",
                column: "AssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelationshipTypes_Associations_AssociationId",
                table: "RelationshipTypes",
                column: "AssociationId",
                principalTable: "Associations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelationshipTypes_Associations_AssociationId",
                table: "RelationshipTypes");

            migrationBuilder.DropIndex(
                name: "IX_RelationshipTypes_AssociationId",
                table: "RelationshipTypes");

            migrationBuilder.DropColumn(
                name: "AssociationId",
                table: "RelationshipTypes");
        }
    }
}
