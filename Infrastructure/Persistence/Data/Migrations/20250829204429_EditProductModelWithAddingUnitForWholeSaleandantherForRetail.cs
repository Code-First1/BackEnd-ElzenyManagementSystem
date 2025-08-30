using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditProductModelWithAddingUnitForWholeSaleandantherForRetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Products",
                newName: "UnitForWholeSale");

            migrationBuilder.RenameColumn(
                name: "PricePerUnit",
                table: "Products",
                newName: "PrieceForWholeSale");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceForRetail",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnitForRetail",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceForRetail",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitForRetail",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UnitForWholeSale",
                table: "Products",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "PrieceForWholeSale",
                table: "Products",
                newName: "PricePerUnit");
        }
    }
}
