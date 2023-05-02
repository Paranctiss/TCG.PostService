using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Domain
{
    public class LikedSearchPost
    {
        public Guid SearchPostId;
        public int UserId;
        public DateTime LikeAt;

        public SearchPost SearchPost;
        
    }
}
