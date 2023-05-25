using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories;

public class SalePostRepository : Repository<SalePost>, ISalePostRepository
{
    protected readonly ServiceDbContext _dbContext;
    public SalePostRepository(ServiceDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<SalePost>> GetAllSalePostPublicAsync<TOrderKey>(
        int pageNumber, int pageSize,
        CancellationToken cancellationToken,
        Expression<Func<SalePost, TOrderKey>> orderBy = null, 
        bool descending = true, 
        Expression<Func<SalePost,bool>> filter = null)
    {
        var query = _dbContext.Set<SalePost>().Include(sp => sp.SalePicturePosts).AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (orderBy != null)
        {
            if (descending)
            {
                query = query.OrderByDescending(orderBy);
            }
            else
            {
                query = query.OrderBy(orderBy);
            }
        }
        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalePost>> GetLastUserSalePostAsync<TOrderKey>(
    int pageSize,
    int userId,
    CancellationToken cancellationToken,
    Expression<Func<SalePost, TOrderKey>> orderBy = null,
    bool descending = true,
    Expression<Func<SalePost, bool>> filter = null)
    {
        var query = _dbContext.Set<SalePost>().Include(sp => sp.SalePicturePosts).AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            if (descending)
            {
                query = query.OrderByDescending(orderBy);
            }
            else
            {
                query = query.OrderBy(orderBy);
            }
        }
        return await query
            .Where(u => u.UserId == userId)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<SalePost> GetSingleSalePostAsync(CancellationToken cancellationToken, Guid id)
    {
        return await _dbContext.SalePosts.Include(sp => sp.SalePicturePosts).FirstOrDefaultAsync(x => x.Id == id);
    }
}