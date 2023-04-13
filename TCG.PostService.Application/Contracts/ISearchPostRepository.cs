using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts;

public interface ISearchPostRepository : IRepository<Domain.SearchPost>
{
    Task<IEnumerable<Domain.SearchPost>> GetAllSearchPostPublicAsync(CancellationToken cancellationToken);
}