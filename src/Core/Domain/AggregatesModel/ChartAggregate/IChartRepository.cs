using System;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ChartAggregate
{
    public interface IChartRepository
    {
        Task<Chart> GetChartAsync(string symbol, string interval, int candles, double precision);
        Task<Chart> GetChartMonthlyAsync(string symbol, string interval, int howManyMonth, double precision);
        Task<Chart> GetChartRangeTimeAsync(string symbol, string interval, DateTime dateFrom, int candles, double precision);
    }
}
