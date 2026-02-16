using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgroSolutions.Property.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingInitialCrops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Crops",
                columns: new[] { "CropId", "Name" },
                values: new object[,]
                {
                    { 1, "Soja" },
                    { 2, "Milho" },
                    { 3, "Feijão" },
                    { 4, "Arroz" },
                    { 5, "Trigo" },
                    { 6, "Algodão" },
                    { 7, "Cana-de-açúcar" },
                    { 8, "Café" },
                    { 9, "Laranja" },
                    { 10, "Tomate" },
                    { 11, "Alface" },
                    { 12, "Girassol" },
                    { 13, "Aveia" },
                    { 14, "Cevada" },
                    { 15, "Batata" },
                    { 16, "Cebola" },
                    { 17, "Uva" },
                    { 18, "Pinho" },
                    { 19, "Capim" },
                    { 20, "Eucalipto" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Crops",
                keyColumn: "CropId",
                keyValue: 20);
        }
    }
}
