using System.Reflection;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCG.Common.MassTransit.Messages;
using TCG.Common.Settings;
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

        config.NewConfig<Domain.SearchPost, SearchPostDtoResponse>()
        .Map(dest => dest.Grading, src => src.Grading);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}
