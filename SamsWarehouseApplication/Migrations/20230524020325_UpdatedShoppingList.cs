using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamsWarehouseApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedShoppingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShoppingListName",
                table: "ShoppingLists",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingListName",
                table: "ShoppingLists");
        }
    }
}
