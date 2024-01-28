using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class accessCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "SearchPosts",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "MerchPosts",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "SearchPosts");

            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "MerchPosts");
        }
    }
}
