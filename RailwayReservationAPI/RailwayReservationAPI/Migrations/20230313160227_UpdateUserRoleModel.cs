using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayReservationAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRoleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityRoleId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdentityRoleId",
                table: "AspNetUsers",
                column: "IdentityRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_IdentityRoleId",
                table: "AspNetUsers",
                column: "IdentityRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_IdentityRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdentityRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdentityRoleId",
                table: "AspNetUsers");
        }
    }
}
