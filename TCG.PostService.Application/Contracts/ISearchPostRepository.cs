using System.Linq.Expressions;
using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts;

public interface ISearchPostRepository : IRepository<Domain.SearchPost>
{
    Task<IEnumerable<Domain.SearchPost>> GetAllSearchPostPublicAsync<TOrderKey>(
        string idReference, 
        string[] idExtensions, 
        string[] idGradings,
        string idUser,
        int pageNumber, int pageSize,
        CancellationToken cancellationToken,
        Expression<Func<Domain.SearchPost, TOrderKey>> orderBy = null,
        bool descending = true,
        Expression<Func<Domain.SearchPost, bool>> filter = null);

    Task<IEnumerable<Domain.SearchPost>> GetLastUserSearchPostAsync<TOrderKey>(
        int userId, 
        int nbMax, 
        CancellationToken cancellationToken,
        Expression<Func<Domain.SearchPost, TOrderKey>> orderBy = null,
        bool descending = true,
        Expression<Func<Domain.SearchPost, bool>> filter = null);

    Task<Domain.SearchPost> GetSingleSearchPostAsync(
        CancellationToken cancellationToken, 
        Guid id);
}