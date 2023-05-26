using System.Reflection;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCG.Common.MassTransit.Messages;
using TCG.Common.Settings;
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
            configure.UsingRabbitMq((context, configurator) =>
            {
                var config = context.GetService<IConfiguration>();
                //On récupère le nom de la table Catalog
                ////On recupère la config de seeting json pour rabbitMQ
                var rabbitMQSettings = config.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                configurator.Host(rabbitMQSettings.Host);
                //Defnir comment les queues sont crées dans rabbit
                configurator.ConfigureEndpoints(context);
            });
            configure.AddRequestClient<PostCreated>();
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
