using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryDetailId",
                table: "ProductCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductCategoryDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryDetail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryDetail", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductCategoryDetail_ProductCategoryDetailId",
                table: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductCategoryDetail");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProductCategoryDetailId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryDetailId",
                table: "ProductCategories");
        }
    }
}
