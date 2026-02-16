using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSolutions.Property.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguredCascadeInPropertyAndField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Crops_CropId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Properties_PropertyId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_Name",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_PropertyId",
                table: "Fields");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_PropertyId_Name",
                table: "Fields",
                columns: new[] { "PropertyId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Crops_CropId",
                table: "Fields",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "CropId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Properties_PropertyId",
                table: "Fields",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Crops_CropId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Properties_PropertyId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_PropertyId_Name",
                table: "Fields");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_Name",
                table: "Fields",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_PropertyId",
                table: "Fields",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Crops_CropId",
                table: "Fields",
                column: "CropId",
                principalTable: "Crops",
                principalColumn: "CropId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Properties_PropertyId",
                table: "Fields",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
