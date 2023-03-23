using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class Adjust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_CarriageTypes_CarriageTypeId",
                table: "Carriages");

            migrationBuilder.AlterColumn<int>(
                name: "CarriageTypeId",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "CarriageTypeId",
                table: "Carriages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Carriages_CarriageTypes_CarriageTypeId",
                table: "Carriages",
                column: "CarriageTypeId",
                principalTable: "CarriageTypes",
                principalColumn: "Id");
        }
    }
}
