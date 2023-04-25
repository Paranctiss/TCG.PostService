using Microsoft.EntityFrameworkCore;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories;

public class SearchPostRepository : Repository<SearchPost>, ISearchPostRepository
{
    protected readonly ServiceDbContext _dbContext;
    public SearchPostRepository(ServiceDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<SearchPost>> GetAllSearchPostPublicAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SearchPosts.Include(s => s.Grading).Where(x => x.IsPublic == true).ToListAsync();
    }
}