using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.LikedSearchPost.DTO.Request;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories
{
    public class LikedSearchPostRepository : Repository<LikedSearchPosts>, ILikedSearchPostRepository
    {
        protected readonly ServiceDbContext _dbContext;
        public LikedSearchPostRepository(ServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LikedSearchPosts>> GetLikedSearchPostsByUserIdAsync(CancellationToken cancellationToken, int userId)
        {
            return await _dbContext.LikedSearchPosts.Include(s => s.SearchPost).Include(g => g.SearchPost.Grading).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<LikedSearchPostDtoRequest> RemoveLikedSearchPosts(LikedSearchPostDtoRequest likedSearchPost, CancellationToken cancellationToken)
        {
            var t = await _dbContext.LikedSearchPosts.Where(s => s.SearchPostId == likedSearchPost.SearchPostId).Where(u => u.UserId == likedSearchPost.UserId).FirstOrDefaultAsync();
            if (t != null)
            {
                _dbContext.LikedSearchPosts.Remove(t);
                await _dbContext.SaveChangesAsync();
            }
            return likedSearchPost;
        }

        public bool IsSearchPostLiked(CancellationToken cancellationToken, int userId, Guid searchPostId)
        {
            if (_dbContext.LikedSearchPosts.Where(s => s.SearchPostId == searchPostId).Where(u => u.UserId == userId).ToList().Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
