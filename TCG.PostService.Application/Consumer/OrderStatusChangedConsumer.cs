using MassTransit;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;

namespace TCG.PostService.Application.Consumer;

public class OrderStatusChangedConsumer : IConsumer<OrderStatusChanged>
{
    private readonly ISalePostRepository _salePostRepository;

    public OrderStatusChangedConsumer(ISalePostRepository salePostRepository)
    {
        _salePostRepository = salePostRepository;
    }

    public async Task Consume(ConsumeContext<OrderStatusChanged> context)
    {
        var message = context.Message;

        if (message != null)
        {
            try
            {
                var post = await _salePostRepository.GetSingleSalePostAsync(context.CancellationToken, message.PostId);
            
                if (post != null)
                {
                    post.StatePostId = message.OrderStatus;
                    await _salePostRepository.UpdateAsync(post, context.CancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }

}