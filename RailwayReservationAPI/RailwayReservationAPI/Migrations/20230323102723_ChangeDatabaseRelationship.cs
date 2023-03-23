using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatabaseRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainTracks");

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_TrainId",
                table: "Tracks",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Trains_TrainId",
                table: "Tracks",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Trains_TrainId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_TrainId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "Tracks");

            migrationBuilder.CreateTable(
                name: "TrainTracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    TrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainTracks_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainTracks_TrackId",
                table: "TrainTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainTracks_TrainId",
                table: "TrainTracks",
                column: "TrainId");
        }
    }
}
