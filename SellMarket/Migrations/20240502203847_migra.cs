using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class migra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductStateId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products",
                column: "ProductStateId",
                principalTable: "ProductStates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductStateId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products",
                column: "ProductStateId",
                principalTable: "ProductStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
