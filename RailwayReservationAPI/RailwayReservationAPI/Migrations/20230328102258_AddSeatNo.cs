using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatNo",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNo",
                table: "Seats");
        }
    }
}
