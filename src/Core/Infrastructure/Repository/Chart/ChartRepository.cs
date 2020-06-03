
namespace Infrastructure.Repository.Chart
{
    using Domain.AggregatesModel.ChartAggregate;
    using System;
    using System.Threading.Tasks;

    public class ChartRepository : IChartRepository
    {
        public Task<Chart> GetChartAsync(string symbol, string interval, int candles, double precision)
        {
            throw new NotImplementedException();
        }

        public Task<Chart> GetChartMonthlyAsync(string symbol, string interval, int howManyMonth, double precision)
        {
            throw new NotImplementedException();
        }

        public Task<Chart> GetChartRangeTimeAsync(string symbol, string interval, DateTime dateFrom, int candles, double precision)
        {
            throw new NotImplementedException();
        }
    }
}
