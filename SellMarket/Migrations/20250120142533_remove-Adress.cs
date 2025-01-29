using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class removeAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserAdresses_UserAdressId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserAdresses");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserAdressId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserAdresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserAdressId",
                table: "Users",
                column: "UserAdressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserAdresses_UserAdressId",
                table: "Users",
                column: "UserAdressId",
                principalTable: "UserAdresses",
                principalColumn: "Id");
        }
    }
}
