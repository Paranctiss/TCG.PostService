using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Application.SearchPost.Query;

namespace TCG.PostService.Application.SalePost.Query;


public record GetSalePostQuery(int id) : IRequest<SalePostDtoResponse>;

public class GetSalePostQueryHandler : IRequestHandler<GetSalePostQuery, SalePostDtoResponse>
{
    private readonly ILogger<GetSalePostQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IMapper _mapper;
    
    public GetSalePostQueryHandler(ILogger<GetSalePostQueryHandler> logger, ISalePostRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<SalePostDtoResponse> Handle(GetSalePostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salePost = await _repository.GetByIdAsync(request.id, cancellationToken);

            if (salePost == null)
            {
                _logger.LogWarning("Search post with id {SalePostId} not found", request.id);
                return null;
            }

            var salePostDtoResponse = _mapper.Map<SalePostDtoResponse>(salePost);

            return salePostDtoResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}