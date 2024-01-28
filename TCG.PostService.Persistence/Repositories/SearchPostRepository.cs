using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using System.Drawing.Printing;
using System.Linq.Expressions;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TCG.PostService.Persistence.Repositories;

public class SearchPostRepository : Repository<SearchPost>, ISearchPostRepository
{
    protected readonly ServiceDbContext _dbContext;
    public SearchPostRepository(ServiceDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<SearchPost>> GetAllSearchPostPublicAsync<TOrderKey>(
        string idReference, 
        string[] idExtensions, 
        string[] idGradings,
        string idUser,
        int pageNumber, int pageSize,
        CancellationToken cancellationToken,
        Expression<Func<SearchPost, TOrderKey>> orderBy = null,
        bool descending = true,
        Expression<Func<SearchPost, bool>> filter = null)
    {
        var query = _dbContext.Set<SearchPost>().Include(s => s.Grading).AsQueryable();

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
        if (idUser != "undefined")
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

        /* var query = _dbContext.Set<SearchPost>()
        .Include(s => s.Grading)
        .Where(x => x.IsPublic &&
            (idReference == "null" || x.ItemId == idReference) &&
            (idUser == "undefined" || x.UserId == int.Parse(idUser)) &&
            (idExtensions[0] == "undefined" || idExtensions.Contains(x.IdExtension)) &&
            (idGradings[0] == "undefined" || idGradings.Contains(x.GradingId.ToString())))
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return await query; */




    }

    public async Task<SearchPost> GetSingleSearchPostAsync(CancellationToken cancellationToken, Guid id)
    {
        return await _dbContext.SearchPosts.Include(s => s.Grading).FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<IEnumerable<SearchPost>> GetLastUserSearchPostAsync<TOrderKey>(
        int userId, 
        int nbMax, 
        CancellationToken cancellationToken,
        Expression<Func<SearchPost, TOrderKey>> orderBy = null,
        bool descending = true,
    Expression<Func<SearchPost, bool>> filter = null)
    {
        var query = _dbContext.Set<SearchPost>().Include(s => s.Grading).AsQueryable();

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
            .Take(nbMax)
            .ToListAsync();
        /* return await _dbContext.SearchPosts.Include(s => s.Grading)
             .Where(x => x.IsPublic == true).Where(u => u.UserId == userId)
             .Take(nbMax).ToListAsync();*/
    }
}