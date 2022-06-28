using Microsoft.Extensions.Caching.Memory;
using PowerCost.API.Models.NordPool;
using PowerCost.API.Models.Prices;

namespace PowerCost.API.Services;

public class NordPoolService : INordPoolService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public NordPoolService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public Task<List<string>?> GetRegions()
    {
        return _cache.GetOrCreateAsync(nameof(GetRegions), async entry =>
        {
            var root = await GetRoot();
            var expiration = CalculateExpirationDate();
            entry.SetAbsoluteExpiration(expiration);
            var result = root.data.Rows.SelectMany(x => x.Columns).Select(x => x.Name).Distinct().ToList();
            return result;
        });
    }

    private static DateTimeOffset CalculateExpirationDate()
    {
        var now = DateTimeOffset.UtcNow;
        now = now.AddHours(1);
        now = now.AddMinutes(-now.Minute);
        now = now.AddSeconds(-now.Second + 10);
        return now;
    }

    public Task<List<Region>?> GetPrices()
    {
        return _cache.GetOrCreateAsync(nameof(GetPrices), async entry =>
        {
            var root = await GetRoot();
            var regions = new Dictionary<string, Region>();
            foreach (var row in root.data.Rows)
            {
                var grouped = row.Columns.GroupBy(x => x.Name);
                foreach (var group in grouped)
                {
                    if (!regions.TryGetValue(group.Key, out var region))
                    {
                        region = new Region { Name = group.Key };
                        regions.Add(group.Key, region);
                    }                   

                    foreach (var column in group)
                    {
                        double.TryParse(column.Value, out var price);
                        var kwhPrice = Math.Round(price / 1000, 3);
                        region.Prices.Add(new Price { StartTime = row.StartTime, EndTime = row.EndTime, Value = kwhPrice, });
                    }
                }
            }
            var expiration = CalculateExpirationDate();
            entry.SetAbsoluteExpiration(expiration);

            return regions.Values.ToList();
        });
    }

    private async Task<Root> GetRoot()
    {
        var result = await _httpClient.GetFromJsonAsync<Root>("https://www.nordpoolgroup.com/api/marketdata/page/23?currency=NOK");
        ArgumentNullException.ThrowIfNull(result);
        return result;
    }
}
