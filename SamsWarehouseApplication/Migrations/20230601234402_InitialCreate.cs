using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SamsWarehouseApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserPasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.AppUserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductUnit = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    ShoppingListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingListName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    ShoppingListDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.ShoppingListId);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItems",
                columns: table => new
                {
                    ShoppingListItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingListId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItems", x => x.ShoppingListItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "ShoppingListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "AppUserId", "UserRole", "UserEmail", "UserPasswordHash" },
                values: new object[,]
                {
                    { 1, "Admin", "test@gmail.com", "$2a$11$R2UbF5jBO34lGDHp7mZyEeWECvTVM6oelzmdC3Uh/TOKl8w83HcMW" },
                    { 2, "User", "customer@gmail.com", "$2a$11$qPCMOY8sASLVh3KQhFCiW.TR9yss6gBPodoasQtSEMafmIpamQpBG" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductName", "ProductPrice", "ProductUnit" },
                values: new object[,]
                {
                    { 1, "Granny Smith Apples", 5.5, "1kg" },
                    { 2, "Fresh Tomatoes", 5.9000000000000004, "500g" },
                    { 3, "Watermelon", 6.5999999999999996, "Whole" },
                    { 4, "Cucumber", 1.8999999999999999, "1 whole" },
                    { 5, "Red Potato Washed", 4.0, "1kg" },
                    { 6, "Red Tipped Bananas", 4.9000000000000004, "1kg" },
                    { 7, "Red Onion", 3.5, "1kg" },
                    { 8, "Carrots", 2.0, "1kg" },
                    { 9, "Iceburg Lettuce", 2.5, "1" },
                    { 10, "Helga's Wholemeal", 3.7000000000000002, "1" },
                    { 11, "Free Range Chicken", 7.5, "1kg" },
                    { 12, "Manning Valley 6-pk", 3.6000000000000001, "6 eggs" },
                    { 13, "A2 Light Milk", 2.8999999999999999, "1 litre" },
                    { 14, "Chobani Strawberry Yoghurt", 1.5, "1" },
                    { 15, "Lurpark Salted Blend", 5.0, "250g" },
                    { 16, "Bega Farmers Tasty", 4.0, "250g" },
                    { 17, "Babybel Mini", 4.2000000000000002, "100g" },
                    { 18, "Cobram EVOO", 8.0, "375ml" },
                    { 19, "Heinz Tomato Soup", 2.5, "535g" },
                    { 20, "John West Tuna can", 1.5, "95g" },
                    { 21, "Cadbury Dairy Milk", 5.0, "200g" },
                    { 22, "Coca Cola", 2.8500000000000001, "2 litre" },
                    { 23, "Smith's Original Share Pack Crisps", 3.29, "170g" },
                    { 24, "Birds Eye Fish Fingers", 4.5, "375g" },
                    { 25, "Berri Orange Juice", 6.0, "2 litre" },
                    { 26, "Vegemite", 6.0, "380g" },
                    { 27, "Cheddar Shapes", 2.0, "175g" },
                    { 28, "Colgate Total ToothPaste", 3.5, "110g" },
                    { 29, "Milo Chocolate Malt", 4.0, "200g" },
                    { 30, "Weet Bix Saniatarium Organic", 5.3300000000000001, "750g" },
                    { 31, "Lindt Excellence 70% Cocoa Block", 4.25, "100g" },
                    { 32, "Original Tim Tams Chocolate", 3.6499999999999999, "200g" },
                    { 33, "Philadelphia Original Cream Cheese", 4.2999999999999998, "250g" },
                    { 34, "Moccona Classic Instant Medium Roast", 6.0, "100g" },
                    { 35, "Capilano Sqeezable Honey", 7.3499999999999996, "500g" },
                    { 36, "Nutella Jar", 4.0, "400g" },
                    { 37, "Arnott's Scotch Finger", 2.8500000000000001, "250g" },
                    { 38, "South Cape Greek Feta", 5.0, "200g" },
                    { 39, "Sacla Pasta Tomato Basil Sauce", 4.5, "420g" },
                    { 40, "Primo English Ham", 3.0, "100g" },
                    { 41, "Primo Short Cut Rindless Bacon", 5.0, "175g" },
                    { 42, "Golden Circle Pinapple Pieces in natural juice", 3.25, "440g" },
                    { 43, "San Renmo Linguine Pasta No 1", 1.95, "500g" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ProductId",
                table: "ShoppingItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ShoppingListId",
                table: "ShoppingItems",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_AppUserId",
                table: "ShoppingLists",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
