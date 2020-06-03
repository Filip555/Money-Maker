using Domain.AggregatesModel.ChartAggregate;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    using Api.Application.Queries;
    public class ChartController : Controller
    {
        private readonly IChartQueries _chartQueries;

        public ChartController(IChartQueries chartQueries)
        {
            _chartQueries = chartQueries;
        }

        [HttpGet("getChart/symbol/{symbol}/inteval/{inteval}/candles/{candles}")]
        public async Task<Chart> GetChart(string symbol, string interval, int candles)
         => await _chartQueries.GetChart(symbol, interval, candles);
    }
}