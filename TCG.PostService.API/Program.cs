using Mapster;
using MapsterMapper;
using TCG.Common.Middlewares.MiddlewareException;
using TCG.Common.MySqlDb;
using TCG.PostService.Application;
using TCG.PostService.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = new TypeAdapterConfig();
builder.Services.AddSingleton(config);
builder.Services.AddApplication();
builder.Services.AddPersistence<ServiceDbContext>(builder.Configuration);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddMassTransitWithRabbitMQ();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureCustomExceptionMiddleware();

app.Run();