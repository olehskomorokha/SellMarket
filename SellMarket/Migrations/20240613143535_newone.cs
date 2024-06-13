using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class newone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductCategoryDetail_ProductCategoryDetailId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProductCategoryDetailId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryDetailId",
                table: "ProductCategories");

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryId",
                table: "ProductCategoryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryDetail_ProductCategoryId",
                table: "ProductCategoryDetail",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryDetail_ProductCategories_ProductCategoryId",
                table: "ProductCategoryDetail",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryDetail_ProductCategories_ProductCategoryId",
                table: "ProductCategoryDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategoryDetail_ProductCategoryId",
                table: "ProductCategoryDetail");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductCategoryDetail");

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryDetailId",
                table: "ProductCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductCategoryDetailId",
                table: "ProductCategories",
                column: "ProductCategoryDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProductCategoryDetail_ProductCategoryDetailId",
                table: "ProductCategories",
                column: "ProductCategoryDetailId",
                principalTable: "ProductCategoryDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
