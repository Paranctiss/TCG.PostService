using System.Collections;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;

namespace TCG.PostService.Application.SalePost.Query;

public record GetSalePostPublicQuery(string idReference, string[] idExtensions, string[] idGradings, string idUser, int pageNumber, int pageSize) : IRequest<IEnumerable<SalePostDtoResponse>>;

public class GetSalePostPublicQueryHandler : IRequestHandler<GetSalePostPublicQuery, IEnumerable<SalePostDtoResponse>>
{
    private readonly ILogger<GetSalePostPublicQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IPictureRepository _pictureRepository;
    private readonly ILikedSalePostRepository _likedSalePostRepository;
    private readonly IMapper _mapper;

    public GetSalePostPublicQueryHandler(
        ILogger<GetSalePostPublicQueryHandler> logger, 
        ISalePostRepository repository, 
        IMapper mapper,
        IPictureRepository pictureRepository,
        ILikedSalePostRepository likedSalePostRepository)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _pictureRepository = pictureRepository;
        _likedSalePostRepository = likedSalePostRepository;
    }
    public async Task<IEnumerable<SalePostDtoResponse>> Handle(GetSalePostPublicQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salePost = await _repository.GetAllSalePostPublicAsync(
                request.idReference,
                request.idExtensions,
                request.idGradings,
                request.idUser,
                request.pageNumber,
                request.pageSize,
                cancellationToken,
                orderBy: x => x.CreatedAt,
                descending: true,
                filter: x => x.IsPublic);

            if (salePost == null)
            {
                _logger.LogWarning("No public search post found");
                return null;
            }

            var salePostDto = _mapper.Map<List<SalePostDtoResponse>>(salePost);

            foreach (SalePostDtoResponse salePostDtoResponse in salePostDto)
            {
                if (_likedSalePostRepository.IsSalePostLiked(cancellationToken, 1, salePostDtoResponse.Id))
                {
                    salePostDtoResponse.Liked = true;
                }
                else
                {
                    salePostDtoResponse.Liked = false;
                }
            }

            return salePostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with public : {ErrorMessage}", ex.Message);
            throw;
        }
    }
}