using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD.SheritonHotel.Data.Migrations
{
    public partial class UpdateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanvasImage",
                table: "Room",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomSize",
                table: "Room",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AmenityType",
                table: "Amenity",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanvasImage",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "RoomSize",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "AmenityType",
                table: "Amenity");
        }
    }
}
