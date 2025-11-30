using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_rental_management_system.Migrations
{
    /// <inheritdoc />
    public partial class editForImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LicenseNumber",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LicenseNumber",
                table: "AspNetUsers",
                column: "LicenseNumber",
                unique: true);
        }
    }
}
