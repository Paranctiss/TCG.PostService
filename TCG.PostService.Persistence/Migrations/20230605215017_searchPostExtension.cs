using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class searchPostExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdExtension",
                table: "SearchPosts",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "LibelleExtension",
                table: "SearchPosts",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdExtension",
                table: "SearchPosts");

            migrationBuilder.DropColumn(
                name: "LibelleExtension",
                table: "SearchPosts");
        }
    }
}
