
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

app.UseSwagger();
app.UseSwaggerUI();

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

app.MapGet("/regions/{region}/prices", (string region, INordPoolService nordPoolService) =>
{
    return nordPoolService.GetPricesForRegion(region);
})
    .WithName("Get all prices for region");

app.MapGet("/regions/{region}/prices/min/{hours}", (string region, int hours, INordPoolService nordPoolService) =>
{
    return nordPoolService.GetCheapestContinuousPeriodForRegion(region, hours);
})
.WithName("Get cheapest period for region");

app.MapGet("/regions/{region}/prices/max/{hours}", (string region, int hours, INordPoolService nordPoolService) =>
{
    return nordPoolService.GetCostliestContinuousPeriodForRegion(region, hours);
})
.WithName("Get costliest period for region");

app.MapGet("regions/{region}/prices/current", (string region, INordPoolService nordPoolService) =>
{
    return nordPoolService.GetCurrentPriceForRegion(region);
})
.WithName("Get current price for region");

app.Run();