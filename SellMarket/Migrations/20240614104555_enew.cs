using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellMarket.Migrations
{
    /// <inheritdoc />
    public partial class enew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserAdresses_UserAdressId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserAdressId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserAdresses_UserAdressId",
                table: "Users",
                column: "UserAdressId",
                principalTable: "UserAdresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserAdresses_UserAdressId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserAdressId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserAdresses_UserAdressId",
                table: "Users",
                column: "UserAdressId",
                principalTable: "UserAdresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
