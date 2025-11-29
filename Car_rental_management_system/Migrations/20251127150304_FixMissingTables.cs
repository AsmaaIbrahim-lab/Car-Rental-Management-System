using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_rental_management_system.Migrations
{
    /// <inheritdoc />
    public partial class FixMissingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarFeature_Cars_CarId",
                table: "Car_CarFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarImage_Cars_CarId",
                table: "Car_CarImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_PricingPlan_PlanId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PricingPlan",
                table: "PricingPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car_CarImage",
                table: "Car_CarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car_CarFeature",
                table: "Car_CarFeature");

            migrationBuilder.RenameTable(
                name: "PricingPlan",
                newName: "Plans");

            migrationBuilder.RenameTable(
                name: "Car_CarImage",
                newName: "Car_Image");

            migrationBuilder.RenameTable(
                name: "Car_CarFeature",
                newName: "Car_Feature");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plans",
                table: "Plans",
                column: "PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car_Image",
                table: "Car_Image",
                columns: new[] { "CarId", "ImagePath" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car_Feature",
                table: "Car_Feature",
                columns: new[] { "CarId", "FeatureName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Feature_Cars_CarId",
                table: "Car_Feature",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Image_Cars_CarId",
                table: "Car_Image",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Plans_PlanId",
                table: "Cars",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Feature_Cars_CarId",
                table: "Car_Feature");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_Image_Cars_CarId",
                table: "Car_Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Plans_PlanId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plans",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car_Image",
                table: "Car_Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car_Feature",
                table: "Car_Feature");

            migrationBuilder.RenameTable(
                name: "Plans",
                newName: "PricingPlan");

            migrationBuilder.RenameTable(
                name: "Car_Image",
                newName: "Car_CarImage");

            migrationBuilder.RenameTable(
                name: "Car_Feature",
                newName: "Car_CarFeature");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PricingPlan",
                table: "PricingPlan",
                column: "PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car_CarImage",
                table: "Car_CarImage",
                columns: new[] { "CarId", "ImagePath" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car_CarFeature",
                table: "Car_CarFeature",
                columns: new[] { "CarId", "FeatureName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarFeature_Cars_CarId",
                table: "Car_CarFeature",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarImage_Cars_CarId",
                table: "Car_CarImage",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_PricingPlan_PlanId",
                table: "Cars",
                column: "PlanId",
                principalTable: "PricingPlan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
