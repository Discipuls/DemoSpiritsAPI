using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitats_MarkerPoints_HabitatId",
                table: "Habitats");

            migrationBuilder.DropIndex(
                name: "IX_Habitats_HabitatId",
                table: "Habitats");

            migrationBuilder.DropColumn(
                name: "HabitatId",
                table: "Habitats");

            migrationBuilder.AddColumn<int>(
                name: "HabitatId",
                table: "MarkerPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MarkerPoints_HabitatId",
                table: "MarkerPoints",
                column: "HabitatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MarkerPoints_Habitats_HabitatId",
                table: "MarkerPoints",
                column: "HabitatId",
                principalTable: "Habitats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarkerPoints_Habitats_HabitatId",
                table: "MarkerPoints");

            migrationBuilder.DropIndex(
                name: "IX_MarkerPoints_HabitatId",
                table: "MarkerPoints");

            migrationBuilder.DropColumn(
                name: "HabitatId",
                table: "MarkerPoints");

            migrationBuilder.AddColumn<int>(
                name: "HabitatId",
                table: "Habitats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Habitats_HabitatId",
                table: "Habitats",
                column: "HabitatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitats_MarkerPoints_HabitatId",
                table: "Habitats",
                column: "HabitatId",
                principalTable: "MarkerPoints",
                principalColumn: "Id");
        }
    }
}
