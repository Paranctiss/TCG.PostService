using System.Linq.Expressions;
using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts;

public interface ISalePostRepository : IRepository<Domain.SalePost>
{
    Task<IEnumerable<Domain.SalePost>> GetAllSalePostPublicAsync<TOrderKey>(
        string idReference,
        string[] idExtensions,
        string[] idGradings,
        int pageNumber, int pageSize,
        CancellationToken cancellationToken,
        Expression<Func<Domain.SalePost, TOrderKey>> orderBy = null,
        bool descending = true,
        Expression<Func<Domain.SalePost, bool>> filter = null);

    Task<IEnumerable<Domain.SalePost>> GetLastUserSalePostAsync<TOrderKey>(
        int pageSize,
        int userId,
        CancellationToken cancellationToken,
        Expression<Func<Domain.SalePost, TOrderKey>> orderBy = null,
        bool descending = true,
        Expression<Func<Domain.SalePost, bool>> filter = null);

    Task<Domain.SalePost> GetSingleSalePostAsync(CancellationToken cancellationToken, Guid id);
}