using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking_clothes.Migrations
{
    public partial class AddProductSizeIdInReserDeti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSizeId",
                table: "ReservationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDetails_ProductSizeId",
                table: "ReservationDetails",
                column: "ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationDetails_ProductSize_ProductSizeId",
                table: "ReservationDetails",
                column: "ProductSizeId",
                principalTable: "ProductSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationDetails_ProductSize_ProductSizeId",
                table: "ReservationDetails");

            migrationBuilder.DropIndex(
                name: "IX_ReservationDetails_ProductSizeId",
                table: "ReservationDetails");

            migrationBuilder.DropColumn(
                name: "ProductSizeId",
                table: "ReservationDetails");
        }
    }
}
