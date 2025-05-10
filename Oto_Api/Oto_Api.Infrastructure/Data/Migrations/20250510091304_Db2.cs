using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oto_Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Db2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Prices",
                newName: "PriceSale");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceIn",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceIn",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "PriceSale",
                table: "Prices",
                newName: "Value");
        }
    }
}
