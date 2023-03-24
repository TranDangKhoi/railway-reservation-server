using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class ForgotDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainCarriage_Carriages_CarriageId",
                table: "TrainCarriage");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainCarriage_Trains_TrainId",
                table: "TrainCarriage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainCarriage",
                table: "TrainCarriage");

            migrationBuilder.RenameTable(
                name: "TrainCarriage",
                newName: "TrainCarriages");

            migrationBuilder.RenameIndex(
                name: "IX_TrainCarriage_TrainId",
                table: "TrainCarriages",
                newName: "IX_TrainCarriages_TrainId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainCarriage_CarriageId",
                table: "TrainCarriages",
                newName: "IX_TrainCarriages_CarriageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainCarriages",
                table: "TrainCarriages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainCarriages_Carriages_CarriageId",
                table: "TrainCarriages",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainCarriages_Trains_TrainId",
                table: "TrainCarriages",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainCarriages_Carriages_CarriageId",
                table: "TrainCarriages");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainCarriages_Trains_TrainId",
                table: "TrainCarriages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainCarriages",
                table: "TrainCarriages");

            migrationBuilder.RenameTable(
                name: "TrainCarriages",
                newName: "TrainCarriage");

            migrationBuilder.RenameIndex(
                name: "IX_TrainCarriages_TrainId",
                table: "TrainCarriage",
                newName: "IX_TrainCarriage_TrainId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainCarriages_CarriageId",
                table: "TrainCarriage",
                newName: "IX_TrainCarriage_CarriageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainCarriage",
                table: "TrainCarriage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainCarriage_Carriages_CarriageId",
                table: "TrainCarriage",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainCarriage_Trains_TrainId",
                table: "TrainCarriage",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
