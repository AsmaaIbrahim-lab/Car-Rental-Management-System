using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_rental_management_system.Migrations
{
    /// <inheritdoc />
    public partial class add_database_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LicenseNumber",
                table: "AspNetUsers",
                column: "LicenseNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LicenseNumber",
                table: "AspNetUsers");
        }
    }
}
