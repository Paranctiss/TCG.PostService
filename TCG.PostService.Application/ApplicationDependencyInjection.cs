using System.Reflection;
using GreenPipes;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCG.Common.MassTransit.Messages;
using TCG.Common.Settings;
using TCG.PostService.Application.Consumer;
using TCG.PostService.Application.SalePost.DTO.Response;
using TCG.PostService.Application.LikedSalePost.DTO.Response;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
       
        return services;
    }
    
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection serviceCollection)
    {
        //Config masstransit to rabbitmq
        serviceCollection.AddMassTransit(configure =>
        {
            configure.AddConsumer<OrderStatusChangedConsumer>();
            configure.UsingRabbitMq((context, configurator) =>
            {
                var config = context.GetService<IConfiguration>();
                //On récupère le nom de la table Catalog
                ////On recupère la config de seeting json pour rabbitMQ
                var rabbitMQSettings = config.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                configurator.Host(rabbitMQSettings.Host);
                
                // Retry policy for consuming messages
                configurator.UseMessageRetry(retryConfig =>
                {
                    // Exponential back-off (second argument is the max retry count)
                    retryConfig.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(3));
                });
                
                //Defnir comment les queues sont crées dans rabbit
                configurator.ConfigureEndpoints(context);
                configurator.ReceiveEndpoint("invoiceservice", e =>
                {
                    e.Consumer<OrderStatusChangedConsumer>(context);
                });
            });
            configure.AddRequestClient<PostCreated>();
            configure.AddRequestClient<UserById>();
        });
        //Start rabbitmq bus pour exanges
        serviceCollection.AddMassTransitHostedService();
        return serviceCollection;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<Domain.SalePost, SalePost.DTO.Response.SalePostDtoResponse>()
            .Map(dest => dest.SalePicturePosts, src => src.SalePicturePosts.Select(p => p.Name));

        config.NewConfig<Domain.SalePost, LikedSalePost.DTO.Response.SalePostDtoResponse>()
            .Map(dest => dest.SalePicturePosts, src => src.SalePicturePosts.Select(p => p.Name));

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
