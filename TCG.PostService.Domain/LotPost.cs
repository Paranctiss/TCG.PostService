using System.ComponentModel.DataAnnotations;

namespace TCG.PostService.Domain;

public class LotPost : MerchPost
{
    public ICollection<SaleLotPost> SaleLotPosts { get; set; }
}