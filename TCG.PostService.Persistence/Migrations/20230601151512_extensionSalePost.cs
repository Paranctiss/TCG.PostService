using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class extensionSalePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdExtension",
                table: "MerchPosts",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LibelleExtension",
                table: "MerchPosts",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdExtension",
                table: "MerchPosts");

            migrationBuilder.DropColumn(
                name: "LibelleExtension",
                table: "MerchPosts");
        }
    }
}
