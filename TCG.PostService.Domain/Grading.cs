namespace TCG.PostService.Domain;

public class Grading
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<SalePost> SalePosts { get; set; }
    public virtual ICollection<SearchPost> SearchPosts { get; set; }
}