using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Consumer.DTO;
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
        string idReference,
        string[] idExtensions,
        string[] idGradings,
        string idUser,
        int pageNumber, int pageSize,
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
        if (idReference != "null")
        {
            query = query.Where(r => r.ItemId == idReference);
        }

        if (idExtensions[0] != "undefined")
        {
            query = query.Where(a => idExtensions.Contains(a.IdExtension));
        }

        if (idGradings[0] != "undefined")
        {
            query = query.Where(g => idGradings.Contains(g.GradingId.ToString()));
        }

        if(idUser != "undefined")
        {
            query = query.Where(u => u.UserId == int.Parse(idUser));
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
        return await _dbContext.SalePosts.Include(sp => sp.SalePicturePosts).Include(s => s.Grading).FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<IEnumerable<MerchPostResponse>> GetAllBuyerSalePostName(CancellationToken cancellationToken, IEnumerable<Guid> id, int buyerId)
    {
        var buyedTransactions = await _dbContext.SalePosts
            .Where(x => id.Contains(x.Id))
            .Select(sp => new MerchPostResponse { MerchPostId = sp.Id, MerchPostName = sp.Name, MerchPostNamePhotos = sp.SalePicturePosts.Select(x => x.Name)})
            .ToListAsync();
        return buyedTransactions;
    }


}