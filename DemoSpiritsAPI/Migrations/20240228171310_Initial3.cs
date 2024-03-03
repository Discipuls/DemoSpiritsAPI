using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitats_GeoPoints_MarkerLocationId",
                table: "Habitats");

            migrationBuilder.DropIndex(
                name: "IX_Habitats_MarkerLocationId",
                table: "Habitats");

            migrationBuilder.DropColumn(
                name: "MarkerLocationId",
                table: "Habitats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarkerLocationId",
                table: "Habitats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Habitats_MarkerLocationId",
                table: "Habitats",
                column: "MarkerLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitats_GeoPoints_MarkerLocationId",
                table: "Habitats",
                column: "MarkerLocationId",
                principalTable: "GeoPoints",
                principalColumn: "Id");
        }
    }
}
