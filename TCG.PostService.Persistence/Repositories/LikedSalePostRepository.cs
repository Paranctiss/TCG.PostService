using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.LikedSalePost.DTO.Request;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories
{
    public class LikedSalePostRepository : Repository<LikedSalePosts>, ILikedSalePostRepository
    {
        protected readonly ServiceDbContext _dbContext;
        public LikedSalePostRepository(ServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<LikedSalePosts>> GetLikedSalePostsByUserIdAsync(CancellationToken cancellationToken, int userId)
        {
            return await _dbContext.LikedSalePosts.Include(s => s.SalePost).Include(g => g.SalePost.Grading).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<LikedSalePostDtoRequest> RemoveLikedSalePosts(LikedSalePostDtoRequest likedSalePost, CancellationToken cancellationToken)
        {
            int t = await _dbContext.LikedSalePosts.Where(s => s.SalePostId == likedSalePost.SalePostId).Where(u => u.UserId == likedSalePost.UserId).ExecuteDeleteAsync();
            return likedSalePost;
        }

        public bool IsSalePostLiked(CancellationToken cancellationToken, int userId, Guid SalePostId)
        {
            if (_dbContext.LikedSalePosts.Where(s => s.SalePostId == SalePostId).Where(u => u.UserId == userId).ToList().Count != 0)
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
