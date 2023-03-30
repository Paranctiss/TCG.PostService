using Microsoft.EntityFrameworkCore;
using TCG.Common.Contracts;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories;

public class OfferPostRepository : Repository<OfferPost>, IOfferPostRepository
{
    public OfferPostRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext)
    {
    }
}