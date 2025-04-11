using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccidentMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccidentCoordinateConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Coordinate_Latitude_Range",
                table: "PolygonCoordinates");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Coordinate_Longitude_Range",
                table: "PolygonCoordinates");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coordinate_Latitude_Range1",
                table: "PolygonCoordinates",
                sql: "[Latitude] >= -90 AND [Latitude] <= 90");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coordinate_Longitude_Range1",
                table: "PolygonCoordinates",
                sql: "[Longitude] >= -180 AND [Longitude] <= 180");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coordinate_Latitude_Range",
                table: "Accidents",
                sql: "[Latitude] >= -90 AND [Latitude] <= 90");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coordinate_Longitude_Range",
                table: "Accidents",
                sql: "[Longitude] >= -180 AND [Longitude] <= 180");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Coordinate_Latitude_Range1",
                table: "PolygonCoordinates");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Coordinate_Longitude_Range1",
                table: "PolygonCoordinates");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Coordinate_Latitude_Range",
                table: "Accidents");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Coordinate_Longitude_Range",
                table: "Accidents");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coordinate_Latitude_Range",
                table: "PolygonCoordinates",
                sql: "[Latitude] >= -90 AND [Latitude] <= 90");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coordinate_Longitude_Range",
                table: "PolygonCoordinates",
                sql: "[Longitude] >= -180 AND [Longitude] <= 180");
        }
    }
}
