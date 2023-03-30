using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class StatePost
{
    [Column(TypeName = "char(1)")]
    public char Id { get; set; }
    public string Name { get; set; }

    public ICollection<SearchPost> SearchPosts { get; set; }

    public ICollection<MerchPost> MerchPosts { get; set; }
}