using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSolutions.Property.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UniqueNamePropertyConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyId_Name",
                table: "Properties",
                columns: new[] { "PropertyId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyId_Name",
                table: "Properties");
        }
    }
}
