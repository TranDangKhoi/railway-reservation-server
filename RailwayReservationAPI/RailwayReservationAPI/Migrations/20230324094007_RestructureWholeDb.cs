using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class RestructureWholeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Carriages_CarriageId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_CarriageId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Carriages_TrainId",
                table: "Carriages");

            migrationBuilder.DropColumn(
                name: "CarriageId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "TotalSeats",
                table: "Carriages");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "Carriages");

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
                        name: "FK_CarriageSeat_Carriages_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Carriages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainCarriage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    CarriageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainCarriage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainCarriage_Carriages_CarriageId",
                        column: x => x.CarriageId,
                        principalTable: "Carriages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainCarriage_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarriageSeat_SeatId",
                table: "CarriageSeat",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainCarriage_CarriageId",
                table: "TrainCarriage",
                column: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainCarriage_TrainId",
                table: "TrainCarriage",
                column: "TrainId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarriageSeat");

            migrationBuilder.DropTable(
                name: "TrainCarriage");

            migrationBuilder.AddColumn<int>(
                name: "CarriageId",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSeats",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CarriageId",
                table: "Seats",
                column: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_Carriages_TrainId",
                table: "Carriages",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Carriages_CarriageId",
                table: "Seats",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
