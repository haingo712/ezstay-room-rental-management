using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AmenityAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmenityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.AmenityId);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "AmenityId", "AmenityName", "OwnerId" },
                values: new object[,]
                {
                    { 1, "WiFi", new Guid("e8447baa-b99d-41ab-a0d7-ddaee80e2ff3") },
                    { 2, "Air Conditioning", new Guid("e8447baa-b99d-41ab-a0d7-ddaee80e2ff3") },
                    { 3, "Heating", new Guid("5b8d129c-414d-4035-9c42-d38d733c8305") },
                    { 4, "Parking", new Guid("5b8d129c-414d-4035-9c42-d38d733c8305") },
                    { 5, "Laundry", new Guid("5b8d129c-414d-4035-9c42-d38d733c8305") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");
        }
    }
}
