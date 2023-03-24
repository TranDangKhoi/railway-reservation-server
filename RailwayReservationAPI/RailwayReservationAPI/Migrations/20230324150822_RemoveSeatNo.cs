using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeatNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarriageSeat");

            migrationBuilder.RenameColumn(
                name: "SeatNo",
                table: "Seats",
                newName: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CarriageId",
                table: "Seats",
                column: "CarriageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Carriages_CarriageId",
                table: "Seats",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Carriages_CarriageId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_CarriageId",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "CarriageId",
                table: "Seats",
                newName: "SeatNo");

            migrationBuilder.CreateTable(
                name: "CarriageSeat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    CarriageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarriageSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarriageSeat_Carriages_CarriageId",
                        column: x => x.CarriageId,
                        principalTable: "Carriages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarriageSeat_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarriageSeat_CarriageId",
                table: "CarriageSeat",
                column: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_CarriageSeat_SeatId",
                table: "CarriageSeat",
                column: "SeatId");
        }
    }
}
