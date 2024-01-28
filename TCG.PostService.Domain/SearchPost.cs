using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class SearchPost
{
    public Guid Id { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public string AccessCode { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string IdExtension { get; set; }
    public string LibelleExtension { get; set; }
    public ICollection<OfferPost> OfferPosts { get; set; }
    public virtual Grading Grading { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int GradingId { get; set; }

    [Column(TypeName = "char(1)")]
    public char StatePostId { get; set; }
    public virtual StatePost StatePost { get; set; }

    public ICollection<LikedSearchPosts> LikedSearchPosts;

    public int UserId { get; set; }
}