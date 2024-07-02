using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiTesting.Migrations
{
    /// <inheritdoc />
    public partial class Intial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AddToCart",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AddToCart");
        }
    }
}
