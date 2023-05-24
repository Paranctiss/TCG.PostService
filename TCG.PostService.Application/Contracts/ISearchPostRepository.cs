using TCG.Common.Contracts;

namespace TCG.PostService.Application.Contracts;

public interface ISearchPostRepository : IRepository<Domain.SearchPost>
{
    Task<IEnumerable<Domain.SearchPost>> GetAllSearchPostPublicAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Domain.SearchPost>> GetLastUserSearchPostAsync(int userId, int nbMax, CancellationToken cancellationToken);

    Task<Domain.SearchPost> GetSingleSearchPostAsync(CancellationToken cancellationToken, Guid id);
}