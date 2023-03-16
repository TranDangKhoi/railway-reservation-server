using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainTracks_TrainId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_TrainId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "Trains");

            migrationBuilder.CreateIndex(
                name: "IX_TrainTracks_TrainId",
                table: "TrainTracks",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainTracks_Trains_TrainId",
                table: "TrainTracks",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainTracks_Trains_TrainId",
                table: "TrainTracks");

            migrationBuilder.DropIndex(
                name: "IX_TrainTracks_TrainId",
                table: "TrainTracks");

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_TrainId",
                table: "Trains",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainTracks_TrainId",
                table: "Trains",
                column: "TrainId",
                principalTable: "TrainTracks",
                principalColumn: "Id");
        }
    }
}
