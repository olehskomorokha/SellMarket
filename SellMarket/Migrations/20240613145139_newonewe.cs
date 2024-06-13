using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class newonewe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductAmount",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductAmount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductAmount",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ProductAmount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
