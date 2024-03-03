using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarkerPoints_Habitats_Id",
                table: "MarkerPoints");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MarkerPoints",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MarkerPoints",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_MarkerPoints_Habitats_Id",
                table: "MarkerPoints",
                column: "Id",
                principalTable: "Habitats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
