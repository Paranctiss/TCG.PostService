using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class OfferStatePost
{
    [Column(TypeName = "char(1)")]
    public char Id { get; set; }
    public string Name { get; set; }
    public ICollection<OfferPost> OfferPosts { get; set; }
}