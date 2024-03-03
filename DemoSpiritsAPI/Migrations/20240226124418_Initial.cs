using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Spirits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Classification = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CardImageName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarkerImageName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spirits", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GeoPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    HabitatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoPoints", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Habitats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MarkerLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habitats_GeoPoints_MarkerLocationId",
                        column: x => x.MarkerLocationId,
                        principalTable: "GeoPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HabitatSpirit",
                columns: table => new
                {
                    HabitatsId = table.Column<int>(type: "int", nullable: false),
                    SpiritsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitatSpirit", x => new { x.HabitatsId, x.SpiritsId });
                    table.ForeignKey(
                        name: "FK_HabitatSpirit_Habitats_HabitatsId",
                        column: x => x.HabitatsId,
                        principalTable: "Habitats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HabitatSpirit_Spirits_SpiritsId",
                        column: x => x.SpiritsId,
                        principalTable: "Spirits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Spirits",
                columns: new[] { "Id", "CardImageName", "Classification", "Description", "MarkerImageName", "Name" },
                values: new object[] { 1, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_GeoPoints_HabitatId",
                table: "GeoPoints",
                column: "HabitatId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitatSpirit_SpiritsId",
                table: "HabitatSpirit",
                column: "SpiritsId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitats_MarkerLocationId",
                table: "Habitats",
                column: "MarkerLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoPoints_Habitats_HabitatId",
                table: "GeoPoints",
                column: "HabitatId",
                principalTable: "Habitats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoPoints_Habitats_HabitatId",
                table: "GeoPoints");

            migrationBuilder.DropTable(
                name: "HabitatSpirit");

            migrationBuilder.DropTable(
                name: "Spirits");

            migrationBuilder.DropTable(
                name: "Habitats");

            migrationBuilder.DropTable(
                name: "GeoPoints");
        }
    }
}
