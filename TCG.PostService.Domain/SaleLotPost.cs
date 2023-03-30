namespace TCG.PostService.Domain;

public class SaleLotPost
{
    public int LotPostId { get; set; }
    public LotPost LotPost { get; set; }

    public int SalePostId { get; set; }
    public SalePost SalePost { get; set; }
}