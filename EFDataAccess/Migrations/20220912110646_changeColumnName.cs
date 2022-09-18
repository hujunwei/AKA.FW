using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDataAccess.Migrations
{
    public partial class changeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_rowVersion",
                table: "People",
                newName: "_RowVersion");

            migrationBuilder.RenameColumn(
                name: "_rowVersion",
                table: "EmailAddresses",
                newName: "_RowVersion");

            migrationBuilder.RenameColumn(
                name: "_rowVersion",
                table: "Addresses",
                newName: "_RowVersion");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmailAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "EmailAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "People");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmailAddresses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EmailAddresses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "_RowVersion",
                table: "People",
                newName: "_rowVersion");

            migrationBuilder.RenameColumn(
                name: "_RowVersion",
                table: "EmailAddresses",
                newName: "_rowVersion");

            migrationBuilder.RenameColumn(
                name: "_RowVersion",
                table: "Addresses",
                newName: "_rowVersion");
        }
    }
}
