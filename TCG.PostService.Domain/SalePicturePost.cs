namespace TCG.PostService.Domain;

public class SalePicturePost
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid SalePostId { get; set; }
    public SalePost SalePost { get; set; }
}