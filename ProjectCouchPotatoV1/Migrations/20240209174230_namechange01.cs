using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCouchPotatoV1.Migrations
{
    /// <inheritdoc />
    public partial class namechange01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "review",
                table: "Reviews",
                newName: "ReviewText");

            migrationBuilder.RenameColumn(
                name: "poster_path",
                table: "Reviews",
                newName: "Poster");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewText",
                table: "Reviews",
                newName: "review");

            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "Reviews",
                newName: "poster_path");
        }
    }
}
