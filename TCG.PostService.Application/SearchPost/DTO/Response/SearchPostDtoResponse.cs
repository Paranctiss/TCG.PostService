using TCG.PostService.Domain;

namespace TCG.PostService.Application.SearchPost.DTO.Response;

public class SearchPostDtoResponse
{
    public Guid Id { get; set; }
    public string ItemId { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public char StatePostId { get; set; }
    public int UserId { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public virtual GradingDtoResponse Grading { get; set; }
    public int GradingId { get; set; }

    public bool Liked { get; set; }
}