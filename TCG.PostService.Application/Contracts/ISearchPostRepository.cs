using TCG.Common.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Contracts;

public interface ISearchPostRepository : IRepository<Domain.SearchPost>
{
    
}