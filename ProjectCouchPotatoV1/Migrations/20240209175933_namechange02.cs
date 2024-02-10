using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCouchPotatoV1.Migrations
{
    /// <inheritdoc />
    public partial class namechange02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "Reviews",
                newName: "poster_path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "poster_path",
                table: "Reviews",
                newName: "Poster");
        }
    }
}
