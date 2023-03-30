using Microsoft.EntityFrameworkCore;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories;

public class SearchPostRepository : Repository<SearchPost>, ISearchPostRepository
{
    public SearchPostRepository(ServiceDbContext dbContext) : base(dbContext)
    {
    }
}