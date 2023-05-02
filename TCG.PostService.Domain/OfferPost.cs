using System.ComponentModel.DataAnnotations.Schema;

namespace TCG.PostService.Domain;

public class OfferPost
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public int BuyerId { get; set; }
    public decimal Price { get; set; }
    
    [Column(TypeName = "char(1)")]
    public char OfferStatePostId { get; set; }
    public OfferStatePost OfferStatePost { get; set; }


    public int MerchPostId { get; set; }
    public MerchPost MerchPost { get; set; }


    public Guid? SearchPostId { get; set; }
    public SearchPost SearchPost { get; set; }
}