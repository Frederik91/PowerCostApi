
using PowerCost.API.Models.NordPool;
using PowerCost.API.Models.Prices;
using PowerCost.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<INordPoolService, NordPoolService>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/regions", (INordPoolService nordPoolService) =>
{
    return nordPoolService.GetRegions();
})
.WithName("GetRegions");
app.MapGet("/prices", (INordPoolService nordPoolService) =>
{
    return nordPoolService.GetPrices();
})
.WithName("GetPrices");

app.Run();