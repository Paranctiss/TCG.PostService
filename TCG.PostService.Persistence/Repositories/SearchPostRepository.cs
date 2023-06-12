using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System.Drawing.Printing;
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

    public async Task<IEnumerable<SearchPost>> GetAllSearchPostPublicAsync(
        string idReference, 
        string[] idExtensions, 
        string[] idGradings,
        string idUser,
        int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {

        var query = _dbContext.Set<SearchPost>()
        .Include(s => s.Grading)
        .Where(x => x.IsPublic &&
            (idReference == "null" || x.ItemId == idReference) &&
            (idUser == "undefined" || x.UserId == int.Parse(idUser)) &&
            (idExtensions[0] == "undefined" || idExtensions.Contains(x.IdExtension)) &&
            (idGradings[0] == "undefined" || idGradings.Contains(x.GradingId.ToString())))
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return await query;

      


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