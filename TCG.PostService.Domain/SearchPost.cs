using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class SearchPost
{
    public Guid Id { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public ICollection<OfferPost> OfferPosts { get; set; }
    public virtual Grading Grading { get; set; }
    public int GradingId { get; set; }

    [Column(TypeName = "char(1)")]
    public char StatePostId { get; set; }
    public virtual StatePost StatePost { get; set; }

    public int UserId { get; set; }
}