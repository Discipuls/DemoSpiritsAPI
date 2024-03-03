using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class BigC2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Habitats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Habitats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Habitats",
                columns: new[] { "Id", "MarkerLocationId", "Name" },
                values: new object[,]
                {
                    { 1, null, null },
                    { 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Spirits",
                columns: new[] { "Id", "CardImageName", "Classification", "Description", "MarkerImageName", "Name" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null },
                    { 2, null, null, null, null, null }
                });
        }
    }
}
