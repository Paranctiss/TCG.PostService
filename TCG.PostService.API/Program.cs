using Mapster;
using MapsterMapper;
using TCG.Common.Middlewares.MiddlewareException;
using TCG.Common.MySqlDb;
using TCG.PostService.Application;
using TCG.PostService.Persistence;


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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddMapper();
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

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.ConfigureCustomExceptionMiddleware();

app.Run();