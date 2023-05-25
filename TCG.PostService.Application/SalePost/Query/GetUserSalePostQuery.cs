using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;

namespace TCG.PostService.Application.SalePost.Query;

public record GetUserSalePostQuery(int pageSize, int userId) : IRequest<IEnumerable<SalePostDtoResponse>>;

public class GetUserSalePostQueryHandler : IRequestHandler<GetUserSalePostQuery, IEnumerable<SalePostDtoResponse>>
{
    private readonly ILogger<GetSalePostPublicQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IPictureRepository _pictureRepository;
    private readonly IMapper _mapper;

    public GetUserSalePostQueryHandler(
        ILogger<GetSalePostPublicQueryHandler> logger,
        ISalePostRepository repository,
        IMapper mapper,
        IPictureRepository pictureRepository)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _pictureRepository = pictureRepository;
    }
    public async Task<IEnumerable<SalePostDtoResponse>> Handle(GetUserSalePostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salePost = await _repository.GetLastUserSalePostAsync(
                request.pageSize,
                request.userId,
                cancellationToken,
                orderBy: x => x.CreatedAt,
                descending: true,
                filter: x => x.IsPublic);

            if (salePost == null)
            {
                _logger.LogWarning("No public search post found");
                return null;
            }

            var salePostDto = _mapper.Map<IEnumerable<SalePostDtoResponse>>(salePost);
            return salePostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving sale post with public : {ErrorMessage}", ex.Message);
            throw;
        }
    }
}