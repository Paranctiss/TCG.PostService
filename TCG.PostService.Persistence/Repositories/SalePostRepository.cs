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

    public async Task<IEnumerable<SalePost>> GetAllSalePostPublicAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SalePosts.Where(x => x.IsPublic == true).ToListAsync();
    }
}