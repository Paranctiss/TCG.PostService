using TCG.Common.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Contracts;

public interface ISalePostRepository : IRepository<Domain.SalePost>
{
    Task<IEnumerable<Domain.SalePost>> GetAllSalePostPublicAsync(CancellationToken cancellationToken);
}