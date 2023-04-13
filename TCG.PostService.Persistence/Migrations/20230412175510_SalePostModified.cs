using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SalePostModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MerchPosts",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MerchPosts",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "MerchPosts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MerchPosts");
        }
    }
}
