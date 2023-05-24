using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Application.LikedSalePost.DTO.Response
{
    public class LikedSalePostDtoResponse
    {
        public Guid SalePostId { get; set; }
        public int UserId { get; set; }
        public DateTime LikeAt { get; set; }

        public SalePostDtoResponse SalePost { get; set; }

    }
}
