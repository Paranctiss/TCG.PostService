using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SearchPostGrading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradingId",
                table: "SearchPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SearchPosts_GradingId",
                table: "SearchPosts",
                column: "GradingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchPosts_Gradings_GradingId",
                table: "SearchPosts",
                column: "GradingId",
                principalTable: "Gradings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchPosts_Gradings_GradingId",
                table: "SearchPosts");

            migrationBuilder.DropIndex(
                name: "IX_SearchPosts_GradingId",
                table: "SearchPosts");

            migrationBuilder.DropColumn(
                name: "GradingId",
                table: "SearchPosts");
        }
    }
}
