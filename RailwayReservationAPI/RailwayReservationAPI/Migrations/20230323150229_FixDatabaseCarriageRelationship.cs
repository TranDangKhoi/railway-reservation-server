using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixDatabaseCarriageRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarriageTypes_Carriages_CarriageId",
                table: "CarriageTypes");

            migrationBuilder.DropIndex(
                name: "IX_CarriageTypes_CarriageId",
                table: "CarriageTypes");

            migrationBuilder.DropColumn(
                name: "CarriageId",
                table: "CarriageTypes");

            migrationBuilder.AddColumn<int>(
                name: "CarriageTypeId",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carriages_CarriageTypeId",
                table: "Carriages",
                column: "CarriageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carriages_CarriageTypes_CarriageTypeId",
                table: "Carriages",
                column: "CarriageTypeId",
                principalTable: "CarriageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_CarriageTypes_CarriageTypeId",
                table: "Carriages");

            migrationBuilder.DropIndex(
                name: "IX_Carriages_CarriageTypeId",
                table: "Carriages");

            migrationBuilder.DropColumn(
                name: "CarriageTypeId",
                table: "Carriages");

            migrationBuilder.AddColumn<int>(
                name: "CarriageId",
                table: "CarriageTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarriageTypes_CarriageId",
                table: "CarriageTypes",
                column: "CarriageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarriageTypes_Carriages_CarriageId",
                table: "CarriageTypes",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
