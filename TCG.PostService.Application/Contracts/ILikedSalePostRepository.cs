using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.Contracts;
using TCG.PostService.Application.LikedSalePost.DTO.Request;

namespace TCG.PostService.Application.Contracts
{
    public interface ILikedSalePostRepository : IRepository<Domain.LikedSalePosts>
    {
        Task<IEnumerable<Domain.LikedSalePosts>> GetLikedSalePostsByUserIdAsync(CancellationToken cancellationToken, int userId);

        Task<LikedSalePostDtoRequest> RemoveLikedSalePosts(LikedSalePostDtoRequest likedSalePost, CancellationToken cancellationToken);

        bool IsSalePostLiked(CancellationToken cancellationToken, int userId, Guid SalePostId);
        
    }
}
