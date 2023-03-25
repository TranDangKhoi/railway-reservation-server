using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainCarriages");

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages");

            migrationBuilder.DropIndex(
                name: "IX_Carriages_TrainId",
                table: "Carriages");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "Carriages");

            migrationBuilder.CreateTable(
                name: "TrainCarriages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarriageId = table.Column<int>(type: "int", nullable: false),
                    TrainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainCarriages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainCarriages_Carriages_CarriageId",
                        column: x => x.CarriageId,
                        principalTable: "Carriages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainCarriages_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainCarriages_CarriageId",
                table: "TrainCarriages",
                column: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainCarriages_TrainId",
                table: "TrainCarriages",
                column: "TrainId");
        }
    }
}
