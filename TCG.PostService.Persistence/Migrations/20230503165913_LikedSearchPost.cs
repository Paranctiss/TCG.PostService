using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LikedSearchPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikedSearchPost",
                columns: table => new
                {
                    SearchPostId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedSearchPost", x => new { x.SearchPostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LikedSearchPost_SearchPosts_SearchPostId",
                        column: x => x.SearchPostId,
                        principalTable: "SearchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedSearchPost");
        }
    }
}
