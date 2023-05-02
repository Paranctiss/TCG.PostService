using TCG.Common.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Contracts;

public interface IGradingRepository : IRepository<Domain.Grading>
{
    Task<Grading> GetDetachedByIdAsync(int id, CancellationToken cancellationToken);
}