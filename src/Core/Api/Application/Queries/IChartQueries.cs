using Domain.AggregatesModel.ChartAggregate;
using System.Threading.Tasks;

namespace Api.Application.Queries
{
    public interface IChartQueries
    {
        Task<Chart> GetChart(string symbol, string interval, int candles);
    }
}
