using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Gradings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradings", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfferStatePosts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(1)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferStatePosts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RewardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    DropRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StatePosts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(1)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatePosts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    RewardItemId = table.Column<int>(type: "int", nullable: false),
                    RewardTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rewards_RewardTypes_RewardTypeId",
                        column: x => x.RewardTypeId,
                        principalTable: "RewardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SearchPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ItemId = table.Column<string>(type: "longtext", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "longtext", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    IdExtension = table.Column<string>(type: "longtext", nullable: false),
                    LibelleExtension = table.Column<string>(type: "longtext", nullable: false),
                    GradingId = table.Column<int>(type: "int", nullable: false),
                    StatePostId = table.Column<string>(type: "char(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchPosts_Gradings_GradingId",
                        column: x => x.GradingId,
                        principalTable: "Gradings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchPosts_StatePosts_StatePostId",
                        column: x => x.StatePostId,
                        principalTable: "StatePosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AvailableRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailableRewards_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MerchPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "longtext", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StatePostId = table.Column<string>(type: "char(1)", nullable: false),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<string>(type: "longtext", nullable: true),
                    GradingId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Image = table.Column<string>(type: "longtext", nullable: true),
                    IdExtension = table.Column<string>(type: "longtext", nullable: true),
                    LibelleExtension = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchPosts_Gradings_GradingId",
                        column: x => x.GradingId,
                        principalTable: "Gradings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchPosts_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MerchPosts_StatePosts_StatePostId",
                        column: x => x.StatePostId,
                        principalTable: "StatePosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LikedSearchPosts",
                columns: table => new
                {
                    SearchPostId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedSearchPosts", x => new { x.SearchPostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LikedSearchPosts_SearchPosts_SearchPostId",
                        column: x => x.SearchPostId,
                        principalTable: "SearchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LikedSalePosts",
                columns: table => new
                {
                    SalePostId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedSalePosts", x => new { x.SalePostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LikedSalePosts_MerchPosts_SalePostId",
                        column: x => x.SalePostId,
                        principalTable: "MerchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfferPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfferStatePostId = table.Column<string>(type: "char(1)", nullable: false),
                    MerchPostId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SearchPostId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferPosts_MerchPosts_MerchPostId",
                        column: x => x.MerchPostId,
                        principalTable: "MerchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferPosts_OfferStatePosts_OfferStatePostId",
                        column: x => x.OfferStatePostId,
                        principalTable: "OfferStatePosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferPosts_SearchPosts_SearchPostId",
                        column: x => x.SearchPostId,
                        principalTable: "SearchPosts",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalePicturePosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    SalePostId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePicturePosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalePicturePosts_MerchPosts_SalePostId",
                        column: x => x.SalePostId,
                        principalTable: "MerchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SalePostLots",
                columns: table => new
                {
                    LotPostId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SalePostId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePostLots", x => new { x.LotPostId, x.SalePostId });
                    table.ForeignKey(
                        name: "FK_SalePostLots_MerchPosts_LotPostId",
                        column: x => x.LotPostId,
                        principalTable: "MerchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalePostLots_MerchPosts_SalePostId",
                        column: x => x.SalePostId,
                        principalTable: "MerchPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableRewards_RewardId",
                table: "AvailableRewards",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchPosts_GradingId",
                table: "MerchPosts",
                column: "GradingId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchPosts_RewardId",
                table: "MerchPosts",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchPosts_StatePostId",
                table: "MerchPosts",
                column: "StatePostId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferPosts_MerchPostId",
                table: "OfferPosts",
                column: "MerchPostId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferPosts_OfferStatePostId",
                table: "OfferPosts",
                column: "OfferStatePostId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferPosts_SearchPostId",
                table: "OfferPosts",
                column: "SearchPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_RewardTypeId",
                table: "Rewards",
                column: "RewardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePicturePosts_SalePostId",
                table: "SalePicturePosts",
                column: "SalePostId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePostLots_SalePostId",
                table: "SalePostLots",
                column: "SalePostId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchPosts_GradingId",
                table: "SearchPosts",
                column: "GradingId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchPosts_StatePostId",
                table: "SearchPosts",
                column: "StatePostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableRewards");

            migrationBuilder.DropTable(
                name: "LikedSalePosts");

            migrationBuilder.DropTable(
                name: "LikedSearchPosts");

            migrationBuilder.DropTable(
                name: "OfferPosts");

            migrationBuilder.DropTable(
                name: "SalePicturePosts");

            migrationBuilder.DropTable(
                name: "SalePostLots");

            migrationBuilder.DropTable(
                name: "OfferStatePosts");

            migrationBuilder.DropTable(
                name: "SearchPosts");

            migrationBuilder.DropTable(
                name: "MerchPosts");

            migrationBuilder.DropTable(
                name: "Gradings");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "StatePosts");

            migrationBuilder.DropTable(
                name: "RewardTypes");
        }
    }
}
