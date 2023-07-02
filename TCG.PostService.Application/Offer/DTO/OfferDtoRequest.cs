using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Application.Offer.DTO
{
    public class OfferDtoRequest
    {
        public string SalePostId { get; set; }
        public string ?SearchPostId { get; set; } = null;
        public int BuyerId { get; set; }
        public decimal Price { get; set; }
    }
}
