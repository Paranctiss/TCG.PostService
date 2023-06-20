using System.Text.Json;
using MassTransit;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;

namespace TCG.PostService.Application.Consumer;

public class BuyerTransactionsConsumer : IConsumer<BuyerTransaction>
{
    private readonly ISalePostRepository _salePostRepository;

    public BuyerTransactionsConsumer(ISalePostRepository salePostRepository)
    {
        _salePostRepository = salePostRepository;
    }

    public async Task Consume(ConsumeContext<BuyerTransaction> context)
    {
        var message = context.Message;
        var merchPostIdsDeserialized = JsonSerializer.Deserialize<List<Guid>>(message.MerchId);
        var merchPostResponse =
            await _salePostRepository.GetAllBuyerSalePostName(context.CancellationToken, merchPostIdsDeserialized,
                context.Message.buyerId);
        var merchPostIdsSerialized = JsonSerializer.Serialize(merchPostResponse);
        if (merchPostResponse == null)
        {
            await context.RespondAsync(new BuyerTransactionResponse("ERROR: Response is null",new List<string>()));
        }
        else
        {
            await context.RespondAsync(new BuyerTransactionResponse(merchPostIdsSerialized, new List<string>()));
        }
    }
}