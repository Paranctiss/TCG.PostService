using TCG.PostService.Domain;

namespace TCG.PostService.Application.SalePost.DTO.Response;

public class SalePostDtoResponse
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public int GradingId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual GradingDtoResponse Grading { get; set; }
    public ICollection<string> SalePicturePosts { get; set; }
    public bool Liked { get; set; }
    public string IdExtension { get; set; }
    public string LibelleExtension { get; set; }
    public string Username { get; set; }
}