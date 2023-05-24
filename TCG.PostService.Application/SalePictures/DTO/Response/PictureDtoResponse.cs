using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.PostService.Application.SalePictures.DTO.Response
{
    public class PictureDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid SalePostId { get; set; }

    }
}
