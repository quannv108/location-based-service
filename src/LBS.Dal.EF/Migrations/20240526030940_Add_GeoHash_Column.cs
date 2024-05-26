using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LBS.Dal.EF.Migrations
{
    /// <inheritdoc />
    public partial class Add_GeoHash_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeoHash",
                schema: "lbs",
                table: "NamedLocations",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NamedLocations_GeoHash",
                schema: "lbs",
                table: "NamedLocations",
                column: "GeoHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NamedLocations_GeoHash",
                schema: "lbs",
                table: "NamedLocations");

            migrationBuilder.DropColumn(
                name: "GeoHash",
                schema: "lbs",
                table: "NamedLocations");
        }
    }
}
