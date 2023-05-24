using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Domain
{
    public class LikedSalePosts
    {
        public Guid SalePostId;
        public int UserId;
        public DateTime LikeAt;

        public SalePost SalePost;
        
    }
}
