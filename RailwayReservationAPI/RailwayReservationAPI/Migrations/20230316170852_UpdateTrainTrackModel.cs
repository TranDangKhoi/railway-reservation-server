using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainTrackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainTracks_TrainTrackId",
                table: "Trains");

            migrationBuilder.RenameColumn(
                name: "TrainTrackId",
                table: "Trains",
                newName: "TrainId");

            migrationBuilder.RenameIndex(
                name: "IX_Trains_TrainTrackId",
                table: "Trains",
                newName: "IX_Trains_TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainTracks_TrainId",
                table: "Trains",
                column: "TrainId",
                principalTable: "TrainTracks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainTracks_TrainId",
                table: "Trains");

            migrationBuilder.RenameColumn(
                name: "TrainId",
                table: "Trains",
                newName: "TrainTrackId");

            migrationBuilder.RenameIndex(
                name: "IX_Trains_TrainId",
                table: "Trains",
                newName: "IX_Trains_TrainTrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainTracks_TrainTrackId",
                table: "Trains",
                column: "TrainTrackId",
                principalTable: "TrainTracks",
                principalColumn: "Id");
        }
    }
}
