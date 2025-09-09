using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class MergeDatabasesInOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventoryToShopTransactionItems_InventoryToShopTransactions_TransactionId",
                table: "inventoryToShopTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_inventoryToShopTransactionItems_Products_ProductId",
                table: "inventoryToShopTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Shop_ShopId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesProduct_Invoices_InvoiceId",
                table: "InvoicesProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesProduct_Products_ProductId",
                table: "InvoicesProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Shop_ShopId",
                table: "ShopProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inventoryToShopTransactionItems",
                table: "inventoryToShopTransactionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shop",
                table: "Shop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoicesProduct",
                table: "InvoicesProduct");

            migrationBuilder.RenameTable(
                name: "inventoryToShopTransactionItems",
                newName: "InventoryToShopTransactionItems");

            migrationBuilder.RenameTable(
                name: "Shop",
                newName: "Shops");

            migrationBuilder.RenameTable(
                name: "InvoicesProduct",
                newName: "InvoiceProducts");

            migrationBuilder.RenameIndex(
                name: "IX_inventoryToShopTransactionItems_TransactionId",
                table: "InventoryToShopTransactionItems",
                newName: "IX_InventoryToShopTransactionItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_inventoryToShopTransactionItems_ProductId",
                table: "InventoryToShopTransactionItems",
                newName: "IX_InventoryToShopTransactionItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesProduct_ProductId",
                table: "InvoiceProducts",
                newName: "IX_InvoiceProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesProduct_InvoiceId",
                table: "InvoiceProducts",
                newName: "IX_InvoiceProducts_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryToShopTransactionItems",
                table: "InventoryToShopTransactionItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shops",
                table: "Shops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceProducts",
                table: "InvoiceProducts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryToShopTransactionItems_InventoryToShopTransactions_TransactionId",
                table: "InventoryToShopTransactionItems",
                column: "TransactionId",
                principalTable: "InventoryToShopTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryToShopTransactionItems_Products_ProductId",
                table: "InventoryToShopTransactionItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceProducts_Invoices_InvoiceId",
                table: "InvoiceProducts",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceProducts_Products_ProductId",
                table: "InvoiceProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Shops_ShopId",
                table: "Invoices",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Shops_ShopId",
                table: "ShopProducts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryToShopTransactionItems_InventoryToShopTransactions_TransactionId",
                table: "InventoryToShopTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryToShopTransactionItems_Products_ProductId",
                table: "InventoryToShopTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceProducts_Invoices_InvoiceId",
                table: "InvoiceProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceProducts_Products_ProductId",
                table: "InvoiceProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Shops_ShopId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProducts_Shops_ShopId",
                table: "ShopProducts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryToShopTransactionItems",
                table: "InventoryToShopTransactionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shops",
                table: "Shops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceProducts",
                table: "InvoiceProducts");

            migrationBuilder.RenameTable(
                name: "InventoryToShopTransactionItems",
                newName: "inventoryToShopTransactionItems");

            migrationBuilder.RenameTable(
                name: "Shops",
                newName: "Shop");

            migrationBuilder.RenameTable(
                name: "InvoiceProducts",
                newName: "InvoicesProduct");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryToShopTransactionItems_TransactionId",
                table: "inventoryToShopTransactionItems",
                newName: "IX_inventoryToShopTransactionItems_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryToShopTransactionItems_ProductId",
                table: "inventoryToShopTransactionItems",
                newName: "IX_inventoryToShopTransactionItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceProducts_ProductId",
                table: "InvoicesProduct",
                newName: "IX_InvoicesProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceProducts_InvoiceId",
                table: "InvoicesProduct",
                newName: "IX_InvoicesProduct_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inventoryToShopTransactionItems",
                table: "inventoryToShopTransactionItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shop",
                table: "Shop",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoicesProduct",
                table: "InvoicesProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryToShopTransactionItems_InventoryToShopTransactions_TransactionId",
                table: "inventoryToShopTransactionItems",
                column: "TransactionId",
                principalTable: "InventoryToShopTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_inventoryToShopTransactionItems_Products_ProductId",
                table: "inventoryToShopTransactionItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Shop_ShopId",
                table: "Invoices",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesProduct_Invoices_InvoiceId",
                table: "InvoicesProduct",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesProduct_Products_ProductId",
                table: "InvoicesProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProducts_Shop_ShopId",
                table: "ShopProducts",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
