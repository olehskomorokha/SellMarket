using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class newOnewewe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductStateId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductStateId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductStateId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductStateId",
                table: "Products",
                column: "ProductStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products",
                column: "ProductStateId",
                principalTable: "ProductStates",
                principalColumn: "Id");
        }
    }
}
