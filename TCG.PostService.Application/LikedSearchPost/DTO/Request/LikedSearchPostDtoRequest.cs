using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Application.LikedSearchPost.DTO.Request
{
    public class LikedSearchPostDtoRequest
    {
        public Guid SearchPostId { get; set; }
        public int UserId { get; set; }
    }
}
