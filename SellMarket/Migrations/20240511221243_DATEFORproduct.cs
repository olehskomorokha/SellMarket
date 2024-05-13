using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class DATEFORproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateOfPublish",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfPublish",
                table: "Products");
        }
    }
}
