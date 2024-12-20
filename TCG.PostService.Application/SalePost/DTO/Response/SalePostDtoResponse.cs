namespace TCG.PostService.Application.SalePost.DTO.Response;

public class SalePostDtoResponse
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public int GradingId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
}