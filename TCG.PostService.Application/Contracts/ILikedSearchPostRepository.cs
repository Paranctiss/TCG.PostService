using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.Contracts;
using TCG.PostService.Application.LikedSearchPost.DTO.Request;

namespace TCG.PostService.Application.Contracts
{
    public interface ILikedSearchPostRepository : IRepository<Domain.LikedSearchPosts>
    {
        Task<IEnumerable<Domain.LikedSearchPosts>> GetLikedSearchPostsByUserIdAsync(CancellationToken cancellationToken, int userId);

        Task<LikedSearchPostDtoRequest> RemoveLikedSearchPosts(LikedSearchPostDtoRequest likedSearchPost, CancellationToken cancellationToken);

        bool IsSearchPostLiked(CancellationToken cancellationToken, int userId, Guid searchPostId);
        
    }
}
