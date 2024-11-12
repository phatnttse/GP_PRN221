using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blossom_RazorWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlowerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Flowers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Flowers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FlowerExpireDate",
                table: "Flowers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "FlowerExpireDate",
                table: "Flowers");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Flowers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
