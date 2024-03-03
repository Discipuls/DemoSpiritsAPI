using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class BigC1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "spiritId",
                table: "GeoPoints",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_GeoPoints_spiritId",
                table: "GeoPoints",
                column: "spiritId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoPoints_Spirits_spiritId",
                table: "GeoPoints",
                column: "spiritId",
                principalTable: "Spirits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoPoints_Spirits_spiritId",
                table: "GeoPoints");

            migrationBuilder.DropIndex(
                name: "IX_GeoPoints_spiritId",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "spiritId",
                table: "GeoPoints");

            migrationBuilder.UpdateData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Kuka");

            migrationBuilder.UpdateData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Opernta");
        }
    }
}
