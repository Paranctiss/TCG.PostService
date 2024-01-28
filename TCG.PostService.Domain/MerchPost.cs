using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class MerchPost
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public string AccessCode { get; set; }

    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ICollection<OfferPost> OfferPosts { get; set; }
    
    [Column(TypeName = "char(1)")]
    public char StatePostId { get; set; }
    public StatePost StatePost { get; set; }
}