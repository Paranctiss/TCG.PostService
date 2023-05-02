﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCG.PostService.Persistence;

#nullable disable

namespace TCG.PostService.Persistence.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    partial class ServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TCG.PostService.Domain.AvailableReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("RewardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RewardId");

                    b.ToTable("AvailableRewards");
                });

            modelBuilder.Entity("TCG.PostService.Domain.Grading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Gradings");
                });

            modelBuilder.Entity("TCG.PostService.Domain.LikedSearchPost", b =>
                {
                    b.Property<Guid>("SearchPostId")
                        .HasColumnType("char(36)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SearchPostId", "UserId");

                    b.ToTable("LikedSearchPost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.MerchPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StatePostId")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatePostId");

                    b.ToTable("MerchPosts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("MerchPost");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TCG.PostService.Domain.OfferPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BuyerId")
                        .HasColumnType("int");

                    b.Property<int>("MerchPostId")
                        .HasColumnType("int");

                    b.Property<string>("OfferStatePostId")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("SearchPostId")
                        .HasColumnType("char(36)");

                    b.Property<int>("SellerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MerchPostId");

                    b.HasIndex("OfferStatePostId");

                    b.HasIndex("SearchPostId");

                    b.ToTable("OfferPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.OfferStatePost", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("OfferStatePosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.Reward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RewardItemId")
                        .HasColumnType("int");

                    b.Property<int>("RewardTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RewardTypeId");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("TCG.PostService.Domain.RewardType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("DropRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RewardTypes");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SaleLotPost", b =>
                {
                    b.Property<int>("LotPostId")
                        .HasColumnType("int");

                    b.Property<int>("SalePostId")
                        .HasColumnType("int");

                    b.HasKey("LotPostId", "SalePostId");

                    b.HasIndex("SalePostId");

                    b.ToTable("SalePostLots");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SalePicturePost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SalePostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SalePostId");

                    b.ToTable("SalePicturePosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SearchPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("GradingId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ItemId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StatePostId")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GradingId");

                    b.HasIndex("StatePostId");

                    b.ToTable("SearchPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.StatePost", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("StatePosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.AttribuedReward", b =>
                {
                    b.HasBaseType("TCG.PostService.Domain.MerchPost");

                    b.Property<int?>("RewardId")
                        .HasColumnType("int");

                    b.HasIndex("RewardId");

                    b.HasDiscriminator().HasValue("AttribuedReward");
                });

            modelBuilder.Entity("TCG.PostService.Domain.LotPost", b =>
                {
                    b.HasBaseType("TCG.PostService.Domain.MerchPost");

                    b.HasDiscriminator().HasValue("LotPost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SalePost", b =>
                {
                    b.HasBaseType("TCG.PostService.Domain.MerchPost");

                    b.Property<int>("GradingId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ItemId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasIndex("GradingId");

                    b.HasDiscriminator().HasValue("SalePost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.AvailableReward", b =>
                {
                    b.HasOne("TCG.PostService.Domain.Reward", null)
                        .WithMany("AvailableRewards")
                        .HasForeignKey("RewardId");
                });

            modelBuilder.Entity("TCG.PostService.Domain.LikedSearchPost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.SearchPost", "SearchPost")
                        .WithMany("LikedSearchPosts")
                        .HasForeignKey("SearchPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SearchPost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.MerchPost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.StatePost", "StatePost")
                        .WithMany("MerchPosts")
                        .HasForeignKey("StatePostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StatePost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.OfferPost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.MerchPost", "MerchPost")
                        .WithMany("OfferPosts")
                        .HasForeignKey("MerchPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCG.PostService.Domain.OfferStatePost", "OfferStatePost")
                        .WithMany("OfferPosts")
                        .HasForeignKey("OfferStatePostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCG.PostService.Domain.SearchPost", "SearchPost")
                        .WithMany("OfferPosts")
                        .HasForeignKey("SearchPostId");

                    b.Navigation("MerchPost");

                    b.Navigation("OfferStatePost");

                    b.Navigation("SearchPost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.Reward", b =>
                {
                    b.HasOne("TCG.PostService.Domain.RewardType", "RewardType")
                        .WithMany("Rewards")
                        .HasForeignKey("RewardTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RewardType");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SaleLotPost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.LotPost", "LotPost")
                        .WithMany("SaleLotPosts")
                        .HasForeignKey("LotPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCG.PostService.Domain.SalePost", "SalePost")
                        .WithMany("SaleLotPosts")
                        .HasForeignKey("SalePostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LotPost");

                    b.Navigation("SalePost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SalePicturePost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.SalePost", "SalePost")
                        .WithMany("SalePicturePosts")
                        .HasForeignKey("SalePostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SalePost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SearchPost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.Grading", "Grading")
                        .WithMany("SearchPosts")
                        .HasForeignKey("GradingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TCG.PostService.Domain.StatePost", "StatePost")
                        .WithMany("SearchPosts")
                        .HasForeignKey("StatePostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grading");

                    b.Navigation("StatePost");
                });

            modelBuilder.Entity("TCG.PostService.Domain.AttribuedReward", b =>
                {
                    b.HasOne("TCG.PostService.Domain.Reward", null)
                        .WithMany("AttribuedRewards")
                        .HasForeignKey("RewardId");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SalePost", b =>
                {
                    b.HasOne("TCG.PostService.Domain.Grading", "Grading")
                        .WithMany("SalePosts")
                        .HasForeignKey("GradingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grading");
                });

            modelBuilder.Entity("TCG.PostService.Domain.Grading", b =>
                {
                    b.Navigation("SalePosts");

                    b.Navigation("SearchPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.MerchPost", b =>
                {
                    b.Navigation("OfferPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.OfferStatePost", b =>
                {
                    b.Navigation("OfferPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.Reward", b =>
                {
                    b.Navigation("AttribuedRewards");

                    b.Navigation("AvailableRewards");
                });

            modelBuilder.Entity("TCG.PostService.Domain.RewardType", b =>
                {
                    b.Navigation("Rewards");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SearchPost", b =>
                {
                    b.Navigation("LikedSearchPosts");

                    b.Navigation("OfferPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.StatePost", b =>
                {
                    b.Navigation("MerchPosts");

                    b.Navigation("SearchPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.LotPost", b =>
                {
                    b.Navigation("SaleLotPosts");
                });

            modelBuilder.Entity("TCG.PostService.Domain.SalePost", b =>
                {
                    b.Navigation("SaleLotPosts");

                    b.Navigation("SalePicturePosts");
                });
#pragma warning restore 612, 618
        }
    }
}
