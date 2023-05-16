using System.Linq.Expressions;
using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts;

public interface ISalePostRepository : IRepository<Domain.SalePost>
{
    Task<IEnumerable<Domain.SalePost>> GetAllSalePostPublicAsync<TOrderKey>(
        int pageNumber, int pageSize,
        CancellationToken cancellationToken,
        Expression<Func<Domain.SalePost, TOrderKey>> orderBy = null,
        bool descending = true,
        Expression<Func<Domain.SalePost, bool>> filter = null);
}