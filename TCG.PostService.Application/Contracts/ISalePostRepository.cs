using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts;

public interface ISalePostRepository : IRepository<Domain.SalePost>
{
    Task<IEnumerable<Domain.SalePost>> GetAllSalePostPublicAsync(CancellationToken cancellationToken);
}