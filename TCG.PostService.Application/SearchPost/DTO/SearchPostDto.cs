namespace TCG.PostService.Application.SearchPost.DTO;

public class SearchPostDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
    public bool IsPublic { get; set; }
    public char StatePostId { get; set; }
    public int UserId { get; set; }
}