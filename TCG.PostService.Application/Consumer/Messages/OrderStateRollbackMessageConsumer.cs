using MassTransit;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;

namespace TCG.PostService.Application.Consumer.Messages;

public class OrderStateRollbackMessageConsumer : IConsumer<OrderStateRollbackMessage>
{
    private readonly ISalePostRepository _salePostRepository;
    private readonly ILogger<OrderStateRollbackMessageConsumer> _logger;

    public OrderStateRollbackMessageConsumer(ISalePostRepository salePostRepository, ILogger<OrderStateRollbackMessageConsumer> logger)
    {
        _salePostRepository = salePostRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStateRollbackMessage> context)
    {
        var post = await _salePostRepository.GetSingleSalePostAsync(context.CancellationToken, context.Message.PostId);

        post.StatePostId = 'C';
        await _salePostRepository.UpdateAsync(post, context.CancellationToken);
        
        _logger.LogInformation("Order {Post} Staus reinitialisé Ordered: {Status}", context.Message.PostId, 'C');
    }
}