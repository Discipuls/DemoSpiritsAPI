using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoSpiritsAPI.Migrations
{
    /// <inheritdoc />
    public partial class MoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Kuka");

            migrationBuilder.InsertData(
                table: "Spirits",
                columns: new[] { "Id", "CardImageName", "Classification", "Description", "MarkerImageName", "Name" },
                values: new object[] { 2, null, null, null, null, "Opernta" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Spirits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: null);
        }
    }
}
