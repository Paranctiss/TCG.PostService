using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Application.LikedSearchPost.DTO.Response
{
    public class LikedSearchPostDtoResponse
    {
        public Guid SearchPostId { get; set; }
        public int UserId { get; set; }
        public DateTime LikeAt { get; set; }

        public SearchPostDtoResponse SearchPost { get; set; }

    }
}
