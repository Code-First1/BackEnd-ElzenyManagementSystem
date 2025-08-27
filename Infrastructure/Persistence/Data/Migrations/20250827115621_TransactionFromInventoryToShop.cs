using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionFromInventoryToShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryToShopTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryToShopTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventoryToShopTransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventoryToShopTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventoryToShopTransactionItems_InventoryToShopTransactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "InventoryToShopTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventoryToShopTransactionItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventoryToShopTransactionItems_ProductId",
                table: "inventoryToShopTransactionItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_inventoryToShopTransactionItems_TransactionId",
                table: "inventoryToShopTransactionItems",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventoryToShopTransactionItems");

            migrationBuilder.DropTable(
                name: "InventoryToShopTransactions");
        }
    }
}
