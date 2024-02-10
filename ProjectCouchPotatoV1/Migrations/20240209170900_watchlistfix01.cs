using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCouchPotatoV1.Migrations
{
    /// <inheritdoc />
    public partial class watchlistfix01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Watchlist",
                table: "Watchlist");

            migrationBuilder.RenameTable(
                name: "Watchlist",
                newName: "Reviews");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Watchlist");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Watchlist",
                table: "Watchlist",
                column: "Id");
        }
    }
}
