using TCG.PostService.Application.SalePictures.DTO.Request;

namespace TCG.PostService.Application.SalePost.DTO.Request;

public class SalePostDtoRequest
{
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public int GradingId { get; set; }
    public int UserId { get; set; }
    public char StatePostId { get; set; }
    public List<PictureDtoRequest> Pictures { get; set; }
}