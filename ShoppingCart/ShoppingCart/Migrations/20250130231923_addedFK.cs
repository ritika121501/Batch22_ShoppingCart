using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Migrations
{
    /// <inheritdoc />
    public partial class addedFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingKarts");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ShoppingKarts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ShoppingKarts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingKarts_ApplicationUserId",
                table: "ShoppingKarts",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingKarts_AspNetUsers_ApplicationUserId",
                table: "ShoppingKarts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingKarts_AspNetUsers_ApplicationUserId",
                table: "ShoppingKarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingKarts_ApplicationUserId",
                table: "ShoppingKarts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ShoppingKarts");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ShoppingKarts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ShoppingKarts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
