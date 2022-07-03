

using Moq;
using PowerCost.API.Models.Prices;

namespace PowerCost.API.Tests
{
    public class NordPoolServiceTests
    {
        private static readonly DateTime _baseDate = DateTime.MinValue;

        [Fact]
        public void CalculateCheapestPeriod()
        {
            var cut = new NordPoolService(null!, null!);
            var prices = GetPrices();
            var region = cut.GetCheapestContinuousPeriodForRegion(prices, 2);

            Assert.Equal(_baseDate.AddHours(2), region.Start);
            Assert.Equal(_baseDate.AddHours(4), region.End);
        }

        [Fact]
        public void CalculateCostliestPeriod()
        {
            var cut = new NordPoolService(null!, null!);
            var prices = GetPrices();
            var region = cut.GetCostliestContinuousPeriodForRegion(prices, 2);

            Assert.Equal(_baseDate.AddHours(0), region.Start);
            Assert.Equal(_baseDate.AddHours(2), region.End);
        }

        private static List<Price> GetPrices()
        {
            return new List<Price>
                        {
                            new Price
                            {
                                StartTime = _baseDate,
                                EndTime = _baseDate.AddHours(1),
                                Value = 4
                            },
                            new Price
                            {
                                StartTime = _baseDate.AddHours(1),
                                EndTime = _baseDate.AddHours(2),
                                Value = 4
                            },
                            new Price
                            {
                                StartTime = _baseDate.AddHours(2),
                                EndTime = _baseDate.AddHours(3),
                                Value = 1
                            },
                            new Price
                            {
                                StartTime = _baseDate.AddHours(3),
                                EndTime = _baseDate.AddHours(4),
                                Value = 2
                            },
                        };
        }
    }
}