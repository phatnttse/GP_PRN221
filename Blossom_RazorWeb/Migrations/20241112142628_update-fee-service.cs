using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blossom_RazorWeb.Migrations
{
    /// <inheritdoc />
    public partial class updatefeeservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FeeService",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeeService",
                table: "AspNetUsers");
        }
    }
}
