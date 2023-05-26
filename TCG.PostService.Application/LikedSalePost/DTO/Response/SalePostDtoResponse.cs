using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.LikedSalePost.DTO.Response
{
    public class SalePostDtoResponse
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Remarks { get; set; }
        public bool IsPublic { get; set; }
        public int GradingId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual GradingDtoResponse Grading { get; set; }
        public ICollection<string> SalePicturePosts { get; set; }

        public bool Liked { get; set; }
    }
}
