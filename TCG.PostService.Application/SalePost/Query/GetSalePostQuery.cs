using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Application.SearchPost.Query;

namespace TCG.PostService.Application.SalePost.Query;


public record GetSalePostQuery(Guid id) : IRequest<SalePostDtoResponse>;

public class GetSalePostQueryHandler : IRequestHandler<GetSalePostQuery, SalePostDtoResponse>
{
    private readonly ILogger<GetSalePostQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserById> _requestClient;

    public GetSalePostQueryHandler(ILogger<GetSalePostQueryHandler> logger, ISalePostRepository repository, IMapper mapper, IRequestClient<UserById> requestClient)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _requestClient = requestClient;
    }
    public async Task<SalePostDtoResponse> Handle(GetSalePostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salePost = await _repository.GetSingleSalePostAsync(cancellationToken, request.id);

            if (salePost == null)
            {
                _logger.LogWarning("Search post with id {SalePostId} not found", request.id);
                return null;
            }

            var userById = new UserById(salePost.UserId);
            var userFromAuth = await _requestClient.GetResponse<UserByIdResponse>(userById, cancellationToken);

            var salePostDtoResponse = _mapper.Map<SalePostDtoResponse>(salePost);

            salePostDtoResponse.Username = userFromAuth.Message.username;

            return salePostDtoResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}