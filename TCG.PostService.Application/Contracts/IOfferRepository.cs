using TCG.Common.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Contracts;

public interface IOfferRepository : IRepository<OfferPost>
{
    Task<int> CreateOffer(OfferPost offerPost, CancellationToken cancellationToken);
    Task UpdateOffer(OfferPost offerPost, CancellationToken cancellationToken);
    Task<OfferPost?> GetOfferExistingByMerchPostId(Guid guid, int idUser, CancellationToken cancellationToken);
    Task<OfferPost?> GetOfferById(int offerId, CancellationToken cancellationToken);
}