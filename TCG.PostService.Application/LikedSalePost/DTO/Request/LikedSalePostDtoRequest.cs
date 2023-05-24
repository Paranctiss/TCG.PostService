using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Application.LikedSalePost.DTO.Request
{
    public class LikedSalePostDtoRequest
    {
        public Guid SalePostId { get; set; }
        public int UserId { get; set; }
    }
}
