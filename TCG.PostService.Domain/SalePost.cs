using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class SalePost : MerchPost
{
    public int ItemId { get; set; }

    public int GradingId { get; set; }
    public Grading Grading { get; set; }

    public ICollection<SalePicturePost> SalePicturePosts { get; set; }

    public ICollection<SaleLotPost> SaleLotPosts { get; set; }
}