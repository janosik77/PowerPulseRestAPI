using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerPulseRestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dictionary",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dictionary", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dictionary_key",
                table: "dictionary",
                column: "key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dictionary");
        }
    }
}
