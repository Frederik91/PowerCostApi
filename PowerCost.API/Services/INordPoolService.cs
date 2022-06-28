using PowerCost.API.Models.NordPool;
using PowerCost.API.Models.Prices;

namespace PowerCost.API.Services
{
    public interface INordPoolService
    {
        Task<List<string>?> GetRegions();
        Task<List<Region>?> GetPrices();
    }
}