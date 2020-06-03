using System;
using System.Threading.Tasks;

namespace Api.Application.Queries
{
    using Api.Application.Models;
    using StrategySimulator.Models;

    public interface ISimulatorQueries
    {
        Task<ChartView> NeuralNetworkView();
        Task<ChartView> NeuralNetworkViewSecond();
        Task<ChartView> SimulateView(string symbol, string interval);
        Task<ChartView> EmaView(string symbol, string interval, int candles, DateTime dateFrom, int ema);
        Task<HistorySimulate> SimulateBuyEndDayStrategy(string symbol, string interval);
    }
}
