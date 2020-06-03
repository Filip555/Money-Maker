using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.ChartAggregate;
using Domain.Strategies;
using StrategySimulator.Models;

namespace StrategySimulator
{
    public interface ISimulator
    {
        public HistorySimulate Simulate(Chart chart, IStrategy strategyMain, Account account);
    }
}
