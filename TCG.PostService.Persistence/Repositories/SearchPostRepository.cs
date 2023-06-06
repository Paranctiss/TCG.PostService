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

    public async Task<IEnumerable<SearchPost>> GetAllSearchPostPublicAsync(string idReference, string[] idExtensions, string[] idGradings, CancellationToken cancellationToken)
    {
        return await _dbContext.SearchPosts
               .Include(s => s.Grading)
               .Where(x => x.IsPublic == true 
               && (idReference == "null" || x.ItemId == idReference) 
               && (idExtensions[0] == "undefined" || idExtensions.Contains(x.IdExtension))
               && (idGradings[0] == "undefined" || idGradings.Contains(x.GradingId.ToString())))
               .ToListAsync();
    }

    public async Task<SearchPost> GetSingleSearchPostAsync(CancellationToken cancellationToken, Guid id)
    {
        return await _dbContext.SearchPosts.Include(s => s.Grading).FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<IEnumerable<SearchPost>> GetLastUserSearchPostAsync(int userId, int nbMax, CancellationToken cancellationToken)
    {
        return await _dbContext.SearchPosts.Include(s => s.Grading).Where(x => x.IsPublic == true).Where(u => u.UserId == userId).Take(nbMax).ToListAsync();
    }
}