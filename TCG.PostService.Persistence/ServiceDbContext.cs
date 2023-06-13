using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TCG.Common.Contracts;
using TCG.Common.MySqlDb;
using TCG.Common.Settings;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence;

public class ServiceDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ServiceDbContext()
    {
        
    }
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        Database.Migrate();
    }

    public DbSet<OfferPost> OfferPosts { get; set; }
    public DbSet<MerchPost> MerchPosts { get; set; }
    public DbSet<OfferStatePost> OfferStatePosts { get; set; }
    public DbSet<SearchPost> SearchPosts { get; set; }
    public DbSet<StatePost> StatePosts { get; set; }
    public DbSet<SalePost> SalePosts { get; set; }
    public DbSet<LotPost> LotPosts { get; set; }
    public DbSet<AttribuedReward> AttribuedRewards { get; set; }
    public DbSet<AvailableReward> AvailableRewards { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<SaleLotPost> SalePostLots { get; set; }
    public DbSet<SalePicturePost> SalePicturePosts { get; set; }
    public DbSet<Grading> Gradings { get; set; }
    public DbSet<RewardType> RewardTypes { get; set; }
    public DbSet<LikedSearchPosts> LikedSearchPosts { get; set; }
    public DbSet<LikedSalePosts> LikedSalePosts { get; set; }
    
    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public Task Migrate()
    {
        return base.Database.MigrateAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OfferPost>(entity =>
        {
            modelBuilder.Entity<OfferPost>()
                .HasOne(p => p.OfferStatePost)
                .WithMany(s => s.OfferPosts)
                .HasForeignKey(p => p.OfferStatePostId)
                .IsRequired();
            modelBuilder.Entity<OfferPost>()
                .HasOne(p => p.SearchPost)
                .WithMany(s => s.OfferPosts)
                .HasForeignKey(p => p.SearchPostId)
                .IsRequired(false);

            modelBuilder.Entity<OfferPost>()
                .HasOne(p => p.MerchPost)
                .WithMany(s => s.OfferPosts)
                .HasForeignKey(p => p.MerchPostId)
                .IsRequired();
            
            modelBuilder.Entity<SearchPost>()
                .HasOne(p => p.StatePost)
                .WithMany(s => s.SearchPosts)
                .HasForeignKey(p => p.StatePostId)
                .IsRequired();

            modelBuilder.Entity<SearchPost>()
                .HasOne(p => p.Grading)
                .WithMany(s => s.SearchPosts)
                .HasForeignKey(p => p.GradingId)
                .IsRequired();

            modelBuilder.Entity<MerchPost>()
                .HasOne(p => p.StatePost)
                .WithMany(s => s.MerchPosts)
                .HasForeignKey(p => p.StatePostId)
                .IsRequired();
            modelBuilder.Entity<MerchPost>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<MerchPost>("MerchPost")
                .HasValue<SalePost>("SalePost");

            modelBuilder.Entity<Reward>()
                .HasOne(p => p.RewardType)
                .WithMany(s => s.Rewards)
                .HasForeignKey(p => p.RewardTypeId)
                .IsRequired();
            
            
            modelBuilder.Entity<SalePost>()
                .HasOne(p => p.Grading)
                .WithMany(s => s.SalePosts)
                .HasForeignKey(p => p.GradingId)
                .IsRequired();
            
            modelBuilder.Entity<SalePicturePost>()
                .HasOne(p => p.SalePost)
                .WithMany(s => s.SalePicturePosts)
                .HasForeignKey(p => p.SalePostId)
                .IsRequired();
            
            modelBuilder.Entity<LotPost>();
            
            modelBuilder.Entity<SalePost>();
            
            modelBuilder.Entity<AttribuedReward>();
            
            modelBuilder.Entity<SaleLotPost>()
                .HasKey(ls => new { ls.LotPostId, ls.SalePostId });
            modelBuilder.Entity<SaleLotPost>()
                .HasOne(ls => ls.LotPost)
                .WithMany(lp => lp.SaleLotPosts)
                .HasForeignKey(ls => ls.LotPostId);
            modelBuilder.Entity<SaleLotPost>()
                .HasOne(ls => ls.SalePost)
                .WithMany(sp => sp.SaleLotPosts)
                .HasForeignKey(ls => ls.SalePostId);

            modelBuilder.Entity<LikedSearchPosts>()
            .HasKey(lsp => new { lsp.SearchPostId, lsp.UserId });
            modelBuilder.Entity<LikedSearchPosts>()
            .HasOne(lsp => lsp.SearchPost)
            .WithMany(sp => sp.LikedSearchPosts)
            .HasForeignKey(lsp => lsp.SearchPostId);

            modelBuilder.Entity<LikedSalePosts>()
           .HasKey(lsp => new { lsp.SalePostId, lsp.UserId });
            modelBuilder.Entity<LikedSalePosts>()
            .HasOne(lsp => lsp.SalePost)
            .WithMany(sp => sp.LikedSalePosts)
            .HasForeignKey(lsp => lsp.SalePostId);

        });
    }
}