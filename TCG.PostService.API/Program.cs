using MapsterMapper;
using TCG.Common.Authentification;
using TCG.Common.Middlewares.MiddlewareException;
using TCG.Common.MySqlDb;
using TCG.PostService.Application;
using TCG.PostService.Persistence;
using TCG.PostService.API.SwaggerConfig;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Asp.Versioning;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                            .WithOrigins("http://localhost:8100") // specifying the allowed origin
                            .WithMethods("GET", "POST", "PUT", "DELETE") // defining the allowed HTTP method
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddApiVersioning(options =>
    {
        //indicating whether a default version is assumed when a client does
        // does not provide an API version.
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(1.0);
    })
    .AddApiExplorer(options =>
    {
        // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;

        //indicating whether a default version is assumed when a client does
        // does not provide an API version.
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    // Add a custom operation filter which sets default values
    options.OperationFilter<SwaggerDefaultValues>();
});
builder.Services.AddMapper();
builder.Services.AddPersistence();
builder.Services.AddApplication();
builder.Services.AddPersistence<ServiceDbContext>(builder.Configuration);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddMassTransitWithRabbitMQ();
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.ConfigureCustomExceptionMiddleware();

app.Run();