using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Spirits",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Habitats",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Spirits");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Habitats");

            migrationBuilder.AddColumn<int>(
                name: "spiritId",
                table: "GeoPoints",
                type: "int",
                nullable: true);

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
    }
}
