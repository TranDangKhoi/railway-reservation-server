using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSeatRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarriageSeat_Carriages_SeatId",
                table: "CarriageSeat");

            migrationBuilder.CreateIndex(
                name: "IX_CarriageSeat_CarriageId",
                table: "CarriageSeat",
                column: "CarriageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarriageSeat_Carriages_CarriageId",
                table: "CarriageSeat",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarriageSeat_Seats_SeatId",
                table: "CarriageSeat",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarriageSeat_Carriages_CarriageId",
                table: "CarriageSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_CarriageSeat_Seats_SeatId",
                table: "CarriageSeat");

            migrationBuilder.DropIndex(
                name: "IX_CarriageSeat_CarriageId",
                table: "CarriageSeat");

            migrationBuilder.AddForeignKey(
                name: "FK_CarriageSeat_Carriages_SeatId",
                table: "CarriageSeat",
                column: "SeatId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
