namespace TCG.PostService.Domain;

public class Grading
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<SalePost> SalePosts { get; set; }
}