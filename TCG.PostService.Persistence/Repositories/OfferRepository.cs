using Microsoft.EntityFrameworkCore;
using TCG.Common.MySqlDb;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Persistence.Repositories;

public class OfferRepository : Repository<OfferPost>, IOfferRepository
{
    protected readonly ServiceDbContext _dbContext;
    public OfferRepository(ServiceDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateOffer(OfferPost offerPost, CancellationToken cancellationToken)
    {
        await _dbContext.OfferPosts.AddAsync(offerPost);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return offerPost.Id;
    }

    public async Task<OfferPost?> GetOfferExistingByMerchPostId(Guid guid, int idBuyer, CancellationToken cancellationToken)
    {
        return await _dbContext.OfferPosts.Where(x => x.MerchPostId == guid && x.OfferStatePostId == 'C' && x.BuyerId == idBuyer).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OfferPost?> GetOfferById(int offerId, CancellationToken cancellationToken)
    {
        return await _dbContext.OfferPosts.Where(x => x.Id == offerId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateOffer(OfferPost offerPost, CancellationToken cancellationToken)
    {
        _dbContext.OfferPosts.Update(offerPost);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}