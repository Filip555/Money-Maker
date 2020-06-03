using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    using Api.Application.Models;
    using Api.Application.Queries;
    using StrategySimulator.Models;

    [ApiController]
    [Route("[controller]")]
    public class SimulatorController : ControllerBase
    {
        private readonly ISimulatorQueries _simulatorQueries;

        public SimulatorController(ISimulatorQueries simulatorQueries)
        {
            _simulatorQueries = simulatorQueries;
        }

        [HttpGet("symbol/{symbol}/interval/{interval}")]
        public async Task<HistorySimulate> Simulate(string symbol, string interval)
          => await _simulatorQueries.SimulateBuyEndDayStrategy(symbol, interval);

        [HttpGet("generateViewNeuralNetwork")]
        public async Task<ChartView> GenerateViewNeuralNetwork()
          => await _simulatorQueries.NeuralNetworkView();

        [HttpGet("generateViewNeuralNetwork2")]
        public async Task<ChartView> GenerateViewNeuralNetwork2()
          => await _simulatorQueries.NeuralNetworkViewSecond();

        [HttpGet("simulateView")]
        public async Task<ChartView> SimulateView(string symbol, string interval)
          => await _simulatorQueries.SimulateView(symbol, interval);

        [HttpGet("emaView")]
        public async Task<ChartView> EmaView(string symbol, string interval, int candles, DateTime dateFrom, int ema)
          => await _simulatorQueries.EmaView(symbol, interval, candles, dateFrom, ema);
    }
}