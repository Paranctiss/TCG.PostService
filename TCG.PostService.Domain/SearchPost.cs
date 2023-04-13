using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class SearchPost
{
    public int Id { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public ICollection<OfferPost> OfferPosts { get; set; }

    [Column(TypeName = "char(1)")]
    public char StatePostId { get; set; }
    public StatePost StatePost { get; set; }

    public int UserId { get; set; }
}