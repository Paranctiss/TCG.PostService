using System.Collections;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;

namespace TCG.PostService.Application.SalePost.Query;

public record GetSalePostPublicQuery() : IRequest<IEnumerable<SalePostDtoResponse>>;

public class GetSalePostPublicQueryHandler : IRequestHandler<GetSalePostPublicQuery, IEnumerable<SalePostDtoResponse>>
{
    private readonly ILogger<GetSalePostPublicQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IMapper _mapper;

    public GetSalePostPublicQueryHandler(ILogger<GetSalePostPublicQueryHandler> logger, ISalePostRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SalePostDtoResponse>> Handle(GetSalePostPublicQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salePost = await _repository.GetAllSalePostPublicAsync(cancellationToken);

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
            _logger.LogError("Error retrieving search post with public : {ErrorMessage}", ex.Message);
            throw;
        }
    }
}