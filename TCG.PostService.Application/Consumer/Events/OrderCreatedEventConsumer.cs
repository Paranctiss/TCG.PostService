using MassTransit;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Events;
using TCG.PostService.Application.Contracts;

namespace TCG.PostService.Application.Consumer.Events;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ISalePostRepository _salePostRepository;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;

    public OrderCreatedEventConsumer(ISalePostRepository salePostRepository, ILogger<OrderCreatedEventConsumer> logger)
    {
        _salePostRepository = salePostRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        var post = await _salePostRepository.GetSingleSalePostAsync(context.CancellationToken, message.PostId);
        if (post == null)
        {
            _logger.LogInformation("Post doesnt exists :{MessageCorrelationId}", context.Message.CorrelationId);
            //AJOUTER LE FAILED EVENT
            await context.Publish(new OrderStateChangedFailedEvent(
                CorrelationId: context.Message.CorrelationId,
                ErrorMessage: $"Post not found with product id {context.Message.PostId} and CorrelationId Id :{context.Message.CorrelationId}",
                PostId: context.Message.PostId
            ));
        }
        else
        {
            post.StatePostId = message.Status;
            await _salePostRepository.UpdateAsync(post, context.CancellationToken);
            _logger.LogInformation("Order {Post} Staus has been changed to Ordered: {Status}", context.Message.PostId, context.Message.Status);
            OrderStateChangedEvent orderStateChangedEvent = new OrderStateChangedEvent(
                CorrelationId: context.Message.CorrelationId,
                PostId: context.Message.PostId
            );
            await context.Publish(orderStateChangedEvent);
        }
    }
}