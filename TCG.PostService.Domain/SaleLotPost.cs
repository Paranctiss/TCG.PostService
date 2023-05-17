namespace TCG.PostService.Domain;

public class SaleLotPost
{
    public Guid LotPostId { get; set; }
    public LotPost LotPost { get; set; }

    public Guid SalePostId { get; set; }
    public SalePost SalePost { get; set; }
}