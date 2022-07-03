using PowerCost.API.Models.NordPool;
using PowerCost.API.Models.Prices;

namespace PowerCost.API.Services
{
    public interface INordPoolService
    {
        Task<List<string>?> GetRegions();
        Task<List<Region>?> GetPrices();
        Task<Price> GetCurrentPriceForRegion(string region);
        Task<List<Price>> GetPricesForRegion(string region);
        Task<Period> GetCheapestContinuousPeriodForRegion(string region, int hours);
        Task<Period> GetCostliestContinuousPeriodForRegion(string region, int hours);
    }
}