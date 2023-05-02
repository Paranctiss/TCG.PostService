using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.LikedSearchPost.DTO.Response
{
    public class SearchPostDtoResponse
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsPublic { get; set; }
        public char StatePostId { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public virtual GradingDtoResponse Grading { get; set; }
        public int GradingId { get; set; }

        public bool Liked { get; set; }
    }
}
