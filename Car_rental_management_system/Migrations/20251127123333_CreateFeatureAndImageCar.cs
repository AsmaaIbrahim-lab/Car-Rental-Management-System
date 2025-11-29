using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_rental_management_system.Migrations
{
    /// <inheritdoc />
    public partial class CreateFeatureAndImageCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car_CarFeature",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false),
                    FeatureName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car_CarFeature", x => new { x.CarId, x.FeatureName });
                    table.ForeignKey(
                        name: "FK_Car_CarFeature_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Car_CarImage",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car_CarImage", x => new { x.CarId, x.ImagePath });
                    table.ForeignKey(
                        name: "FK_Car_CarImage_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car_CarFeature");

            migrationBuilder.DropTable(
                name: "Car_CarImage");
        }
    }
}
