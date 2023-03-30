using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSumShit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarriageId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrackId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarriageId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "CartItems");
        }
    }
}
